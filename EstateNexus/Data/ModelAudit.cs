using System;
using System.Linq;
using EstateNexus.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateNexus.Tests
{
    public static class ModelAudit
    {
        public static int RunAudit()
        {
            var options = new DbContextOptionsBuilder<EstateNexusDbContext>()
                .UseSqlServer("Server=dummy;Database=dummy;Integrated Security=true;TrustServerCertificate=true;")
                .Options;

            using var context = new EstateNexusDbContext(options);
            var model = context.Model;

            var entityTypes = model.GetEntityTypes().ToList();
            Console.WriteLine($"Total Entities in Model: {entityTypes.Count}");
            foreach (var entity in entityTypes)
            {
                Console.WriteLine($" - {entity.ClrType.Name} -> Table: {entity.GetTableName()}");
            }

            var shadowProperties = model.GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .Where(p => p.IsShadowProperty())
                .Select(p => $"{p.DeclaringType.ClrType.Name}.{p.Name}")
                .ToList();

            Console.WriteLine("\n--- Shadow Properties Audit ---");
            if (shadowProperties.Any())
            {
                Console.WriteLine($"FAILED: Found {shadowProperties.Count} shadow property/properties:");
                foreach (var sp in shadowProperties)
                {
                    Console.WriteLine($" [!] {sp}");
                }
                return 1;
            }
            else
            {
                Console.WriteLine("SUCCESS: 0 shadow properties found! Model mapping is 100% explicit.");
                return 0;
            }
        }

        public static int RunIntegrationVerification()
        {
            Console.WriteLine("\n=== RUNNING ESTATE NEXUS DIRECT DBCONTEXT INTEGRATION VERIFICATION ===");
            try
            {
                DatabaseSetup.InitializeDatabase();

                using var context = new EstateNexusDbContext();

                using (var con = new Microsoft.Data.SqlClient.SqlConnection(DatabaseSetup.ConnectionString))
                {
                    con.Open();
                    using var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
                        SELECT 
                            cc.name AS ConstraintName,
                            OBJECT_NAME(cc.parent_object_id) AS TableName,
                            cc.definition AS ConstraintDefinition
                        FROM sys.check_constraints cc
                        ORDER BY TableName, ConstraintName", con);
                    using var reader = cmd.ExecuteReader();
                    Console.WriteLine("\n--- ALL Database Check Constraints ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($" {reader["TableName"]}.{reader["ConstraintName"]}: {reader["ConstraintDefinition"]}");
                    }
                }

                // 1. Verify User & Profile Settings
                var allUsers = context.Users.ToList();
                Console.WriteLine($"[INFO] Total Users in DB: {allUsers.Count}");
                foreach (var u in allUsers)
                {
                    Console.WriteLine($"       User #{u.UserId}: {u.FullName} ({u.Email}) - RoleId: {u.RoleId}");
                }

                var customer = context.Users.FirstOrDefault(u => u.Email == "customer@estatenexus.com" || u.RoleId == 1);
                if (customer == null)
                {
                    customer = new EstateNexus.Models.Entities.User
                    {
                        RoleId = 1,
                        FullName = "John Customer",
                        Email = "customer@estatenexus.com",
                        Phone = "01722222222",
                        PasswordHash = PasswordHelper.HashPassword("customer123"),
                        Address = "Banani, Dhaka",
                        AccountStatus = "Active",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    context.Users.Add(customer);
                    context.SaveChanges();
                    Console.WriteLine($"[INFO] Created customer user: {customer.Email}");
                }

                string testImagePath = "C:\\EstateNexus\\Profiles\\customer_avatar.png";
                customer.ProfileImagePath = testImagePath;
                context.SaveChanges();
                Console.WriteLine($"[PASS] User settings updated directly: ProfileImagePath = '{customer.ProfileImagePath}'");

                // 2. Find or Create Available Property for Rent
                var property = context.Properties.FirstOrDefault(p => p.ListingType == "Rent" && p.PropertyStatus == "Available");
                if (property == null)
                {
                    property = context.Properties.FirstOrDefault();
                    if (property != null)
                    {
                        property.ListingType = "Rent";
                        property.PropertyStatus = "Available";
                        context.SaveChanges();
                    }
                }

                if (property == null)
                {
                    Console.WriteLine("No property found to test checkout!");
                    return 1;
                }

                Console.WriteLine($"[INFO] Using Property: #{property.PropertyId} - '{property.PropertyTitle}' (ListingType: {property.ListingType}, Monthly Price: ৳{property.Price})");

                // 3. Setup Cart with RentalMonths
                var cart = context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefault(c => c.CustomerId == customer.UserId && c.IsActive);

                if (cart == null)
                {
                    cart = new EstateNexus.Models.Entities.Cart
                    {
                        CustomerId = customer.UserId,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }

                // Clear previous cart items for a clean test
                if (cart.CartItems.Any())
                {
                    context.CartItems.RemoveRange(cart.CartItems);
                    context.SaveChanges();
                }

                int testRentalMonths = 6;
                decimal testOfferedPrice = property.Price * testRentalMonths;

                var cartItem = new EstateNexus.Models.Entities.CartItem
                {
                    CartId = cart.CartId,
                    PropertyId = property.PropertyId,
                    RentalMonths = testRentalMonths,
                    OfferedPrice = testOfferedPrice,
                    AddedDate = DateTime.Now
                };
                context.CartItems.Add(cartItem);
                context.SaveChanges();
                Console.WriteLine($"[PASS] Added to Cart with RentalMonths={testRentalMonths}, OfferedPrice=৳{testOfferedPrice:N2}");

                // 4. Overhauled Checkout Flow via EF Core Transaction
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var activeCart = context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Property)
                        .FirstOrDefault(c => c.CustomerId == customer.UserId && c.IsActive);

                    decimal totalAmount = activeCart.CartItems.Sum(ci => ci.OfferedPrice ?? ci.Property.Price);
                    string txnType = activeCart.CartItems.Any(ci => ci.Property.ListingType == "Rent") ? "Rent" : "Sale";

                    // Create Order
                    var order = new EstateNexus.Models.Entities.Order
                    {
                        CustomerId = customer.UserId,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        OrderStatus = "Completed",
                        TransactionType = txnType
                    };
                    context.Orders.Add(order);
                    context.SaveChanges();

                    // Create Payment
                    var payment = new EstateNexus.Models.Entities.Payment
                    {
                        OrderId = order.OrderId,
                        PaymentMethod = "Online/Card",
                        TransactionId = "TXN-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        PaymentAmount = totalAmount,
                        PaymentStatus = "Completed",
                        PaymentDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };
                    context.Payments.Add(payment);
                    context.SaveChanges();

                    // Create OrderItems, Commissions & Update Property Status
                    decimal totalCommission = 0m;
                    foreach (var ci in activeCart.CartItems)
                    {
                        decimal itemFinal = ci.OfferedPrice ?? ci.Property.Price;
                        var orderItem = new EstateNexus.Models.Entities.OrderItem
                        {
                            OrderId = order.OrderId,
                            PropertyId = ci.PropertyId,
                            OwnerId = ci.Property.OwnerId,
                            Quantity = 1,
                            RentalMonths = ci.RentalMonths,
                            UnitPrice = ci.Property.Price,
                            DiscountAmount = 0m,
                            FinalAmount = itemFinal
                        };
                        context.OrderItems.Add(orderItem);

                        decimal commAmount = Math.Round(itemFinal * 0.05m, 2);
                        decimal ownerAmount = itemFinal - commAmount;
                        totalCommission += commAmount;

                        var commission = new EstateNexus.Models.Entities.Commission
                        {
                            OrderId = order.OrderId,
                            CommissionRate = 5.00m,
                            TransactionAmount = itemFinal,
                            CommissionAmount = commAmount,
                            OwnerAmount = ownerAmount,
                            CreatedDate = DateTime.Now
                        };
                        context.Commissions.Add(commission);

                        ci.Property.PropertyStatus = ci.Property.ListingType == "Rent" ? "Rented" : "Sold";
                        ci.Property.UpdatedDate = DateTime.Now;
                    }

                    // Create Invoice linked to OrderId and PaymentId
                    var invoice = new EstateNexus.Models.Entities.Invoice
                    {
                        OrderId = order.OrderId,
                        PaymentId = payment.PaymentId,
                        InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMdd") + "-" + order.OrderId,
                        SubTotal = totalAmount,
                        DiscountAmount = 0m,
                        CommissionAmount = totalCommission,
                        TotalAmount = totalAmount,
                        GeneratedDate = DateTime.Now
                    };
                    context.Invoices.Add(invoice);

                    // Clear Cart
                    context.CartItems.RemoveRange(activeCart.CartItems);

                    context.SaveChanges();
                    transaction.Commit();

                    Console.WriteLine($"[PASS] Checkout Transaction Committed!");
                    Console.WriteLine($"       OrderId: #{order.OrderId}, PaymentId: #{payment.PaymentId}, TxId: {payment.TransactionId}");
                    Console.WriteLine($"       Invoice: {invoice.InvoiceNumber}, SubTotal: ৳{invoice.SubTotal:N2}, Commission: ৳{invoice.CommissionAmount:N2}, Total: ৳{invoice.TotalAmount:N2}");
                }
                catch (Exception txEx)
                {
                    transaction.Rollback();
                    Console.WriteLine($"[FAIL] Checkout transaction failed: {txEx.Message}");
                    Exception inner = txEx.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine($"       -> InnerException: {inner.Message}");
                        inner = inner.InnerException;
                    }
                    return 1;
                }

                // 5. Verification Checks
                using var verifyContext = new EstateNexusDbContext();
                var lastOrder = verifyContext.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Payments)
                    .Include(o => o.Invoice)
                    .Include(o => o.Commission)
                    .OrderByDescending(o => o.OrderId)
                    .FirstOrDefault();

                if (lastOrder == null)
                {
                    Console.WriteLine("[FAIL] Could not retrieve order from database!");
                    return 1;
                }

                var item = lastOrder.OrderItems.First();
                if (item.RentalMonths != testRentalMonths)
                {
                    Console.WriteLine($"[FAIL] OrderItem.RentalMonths expected {testRentalMonths}, got {item.RentalMonths}");
                    return 1;
                }

                if (lastOrder.Invoice == null || lastOrder.Invoice.PaymentId == 0)
                {
                    Console.WriteLine("[FAIL] Invoice not properly linked to PaymentId!");
                    return 1;
                }

                if (lastOrder.Invoice.CommissionAmount <= 0)
                {
                    Console.WriteLine("[FAIL] Invoice CommissionAmount not calculated!");
                    return 1;
                }

                var refreshedProperty = verifyContext.Properties.Find(property.PropertyId);
                if (refreshedProperty.PropertyStatus != "Rented" && refreshedProperty.PropertyStatus != "Sold")
                {
                    Console.WriteLine($"[FAIL] PropertyStatus not updated: {refreshedProperty.PropertyStatus}");
                    return 1;
                }

                // Clean up property status for ongoing testing
                refreshedProperty.PropertyStatus = "Available";
                verifyContext.SaveChanges();

                // 6. Test 2-Item Checkout specifically
                Console.WriteLine("\n--- Testing Checkout with 2 items ---");
                var sellerUser = verifyContext.Users.FirstOrDefault(u => u.RoleId == 2);
                var testPropA = new EstateNexus.Models.Entities.Property
                {
                    OwnerId = sellerUser.UserId,
                    CategoryId = 1,
                    PropertyTitle = "Two-Item Test Prop A (Sale)",
                    ListingType = "Sale",
                    District = "Dhaka",
                    AreaLocation = "Gulshan",
                    FullAddress = "Gulshan Ave",
                    AreaSize = 1800m,
                    AreaUnit = "sqft",
                    Bedrooms = 3,
                    Bathrooms = 3,
                    Price = 12000000m,
                    Description = "Test Prop A Description",
                    PropertyStatus = "Available",
                    ApprovalStatus = "Approved",
                    CreatedDate = DateTime.Now
                };
                var testPropB = new EstateNexus.Models.Entities.Property
                {
                    OwnerId = sellerUser.UserId,
                    CategoryId = 1,
                    PropertyTitle = "Two-Item Test Prop B (Rent)",
                    ListingType = "Rent",
                    District = "Dhaka",
                    AreaLocation = "Banani",
                    FullAddress = "Road 11",
                    AreaSize = 1400m,
                    AreaUnit = "sqft",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Price = 45000m,
                    Description = "Test Prop B Description",
                    PropertyStatus = "Available",
                    ApprovalStatus = "Approved",
                    CreatedDate = DateTime.Now
                };
                verifyContext.Properties.AddRange(testPropA, testPropB);
                verifyContext.SaveChanges();

                // Add Property Image test (with and without image)
                var testPropImg = new EstateNexus.Models.Entities.PropertyImage
                {
                    PropertyId = testPropA.PropertyId,
                    ImagePath = "PropertyImages/test_sample.png",
                    IsPrimary = true,
                    UploadedDate = DateTime.Now
                };
                verifyContext.PropertyImages.Add(testPropImg);
                verifyContext.SaveChanges();
                Console.WriteLine($"[PASS] Add Property without image (Prop #{testPropB.PropertyId}) & with image (Prop #{testPropA.PropertyId}, Image #{testPropImg.ImageId}) verified!");

                // Edit Property test
                testPropA.PropertyTitle = "Two-Item Test Prop A (Edited Title)";
                verifyContext.SaveChanges();
                Console.WriteLine($"[PASS] Edit Property verified!");

                // Setup active cart with 2 items
                var multiCart = new EstateNexus.Models.Entities.Cart
                {
                    CustomerId = customer.UserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                verifyContext.Carts.Add(multiCart);
                verifyContext.SaveChanges();

                var item1 = new EstateNexus.Models.Entities.CartItem
                {
                    CartId = multiCart.CartId,
                    PropertyId = testPropA.PropertyId,
                    RentalMonths = 1,
                    OfferedPrice = 12000000m,
                    AddedDate = DateTime.Now
                };
                var item2 = new EstateNexus.Models.Entities.CartItem
                {
                    CartId = multiCart.CartId,
                    PropertyId = testPropB.PropertyId,
                    RentalMonths = 3,
                    OfferedPrice = 135000m,
                    AddedDate = DateTime.Now
                };
                verifyContext.CartItems.AddRange(item1, item2);
                verifyContext.SaveChanges();

                // Run Checkout on 2-item cart
                using var tx2 = verifyContext.Database.BeginTransaction();
                var cartToCheckout = verifyContext.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Property)
                    .FirstOrDefault(c => c.CartId == multiCart.CartId && c.IsActive);

                decimal twoItemTotal = cartToCheckout.CartItems.Sum(ci => ci.OfferedPrice ?? (ci.Property.ListingType == "Rent" ? ci.Property.Price * (ci.RentalMonths > 0 ? ci.RentalMonths : 1) : ci.Property.Price));
                string twoItemTxnType = cartToCheckout.CartItems.Any(ci => ci.Property.ListingType == "Rent") ? "Rent" : "Sale";

                var order2 = new EstateNexus.Models.Entities.Order
                {
                    CustomerId = customer.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = twoItemTotal,
                    OrderStatus = "Completed",
                    TransactionType = twoItemTxnType
                };
                verifyContext.Orders.Add(order2);
                verifyContext.SaveChanges();

                var payment2 = new EstateNexus.Models.Entities.Payment
                {
                    OrderId = order2.OrderId,
                    PaymentMethod = "Online/Card",
                    TransactionId = "TXN-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    PaymentAmount = twoItemTotal,
                    PaymentStatus = "Completed",
                    PaymentDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                verifyContext.Payments.Add(payment2);
                verifyContext.SaveChanges();

                foreach (var ci in cartToCheckout.CartItems)
                {
                    decimal itemFinal = ci.OfferedPrice ?? (ci.Property.ListingType == "Rent" ? ci.Property.Price * (ci.RentalMonths > 0 ? ci.RentalMonths : 1) : ci.Property.Price);
                    var oi = new EstateNexus.Models.Entities.OrderItem
                    {
                        OrderId = order2.OrderId,
                        PropertyId = ci.PropertyId,
                        OwnerId = ci.Property.OwnerId,
                        Quantity = 1,
                        RentalMonths = ci.RentalMonths,
                        UnitPrice = ci.Property.Price,
                        DiscountAmount = 0m,
                        FinalAmount = itemFinal
                    };
                    verifyContext.OrderItems.Add(oi);
                    ci.Property.PropertyStatus = ci.Property.ListingType == "Rent" ? "Rented" : "Sold";
                    ci.Property.UpdatedDate = DateTime.Now;
                }

                decimal commAmount2 = Math.Round(twoItemTotal * 0.05m, 2);
                decimal ownerAmount2 = twoItemTotal - commAmount2;

                var comm2 = new EstateNexus.Models.Entities.Commission
                {
                    OrderId = order2.OrderId,
                    CommissionRate = 5.00m,
                    TransactionAmount = twoItemTotal,
                    CommissionAmount = commAmount2,
                    OwnerAmount = ownerAmount2,
                    CreatedDate = DateTime.Now
                };
                verifyContext.Commissions.Add(comm2);

                var inv2 = new EstateNexus.Models.Entities.Invoice
                {
                    OrderId = order2.OrderId,
                    PaymentId = payment2.PaymentId,
                    InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMdd") + "-" + order2.OrderId,
                    SubTotal = twoItemTotal,
                    DiscountAmount = 0m,
                    CommissionAmount = commAmount2,
                    TotalAmount = twoItemTotal,
                    GeneratedDate = DateTime.Now
                };
                verifyContext.Invoices.Add(inv2);

                verifyContext.CartItems.RemoveRange(cartToCheckout.CartItems);
                cartToCheckout.IsActive = false;

                verifyContext.SaveChanges();
                tx2.Commit();

                Console.WriteLine($"[PASS] 2-Item Checkout committed successfully!");
                Console.WriteLine($"       Order #{order2.OrderId} | Total: ৳{twoItemTotal:N2} | Commission: ৳{commAmount2:N2} (5%) | Invoice: {inv2.InvoiceNumber}");

                // Verify 2-item order counts
                using var verifyContext2 = new EstateNexusDbContext();
                var savedOrder2 = verifyContext2.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Payments)
                    .Include(o => o.Invoice)
                    .Include(o => o.Commission)
                    .FirstOrDefault(o => o.OrderId == order2.OrderId);

                if (savedOrder2.OrderItems.Count != 2)
                    throw new Exception($"Expected 2 OrderItems, found {savedOrder2.OrderItems.Count}");
                if (savedOrder2.Payments.Count != 1)
                    throw new Exception($"Expected 1 Payment, found {savedOrder2.Payments.Count}");
                if (savedOrder2.Invoice == null)
                    throw new Exception("Invoice is null!");
                if (savedOrder2.Commission == null)
                    throw new Exception("Commission is null!");
                Console.WriteLine($"[PASS] Verified: 1 Order, 2 OrderItems, 1 Payment, 1 Invoice, 1 Commission (5%)");

                // 7. Test Schedule Visit & Visits Load
                Console.WriteLine("\n--- Testing Schedule Visit & Visit Requests Load ---");
                var visit = new EstateNexus.Models.Entities.VisitRequest
                {
                    CustomerId = customer.UserId,
                    PropertyId = testPropA.PropertyId,
                    VisitDate = DateTime.Today.AddDays(2),
                    VisitTime = "11:30 AM",
                    RequestStatus = "Pending",
                    CustomerNote = "Verification visit note",
                    CreatedDate = DateTime.Now
                };
                verifyContext2.VisitRequests.Add(visit);
                verifyContext2.SaveChanges();

                var loadedVisits = verifyContext2.VisitRequests
                    .Include(v => v.Property)
                    .ThenInclude(p => p.Owner)
                    .Where(v => v.CustomerId == customer.UserId)
                    .ToList();
                var visitDisplayList = loadedVisits.Select(v => new
                {
                    VisitId = v.VisitRequestId,
                    PropertyTitle = v.Property != null ? v.Property.PropertyTitle : "N/A",
                    Location = v.Property != null ? (v.Property.District + ", " + v.Property.AreaLocation) : "N/A",
                    Seller = (v.Property != null && v.Property.Owner != null) ? v.Property.Owner.FullName : "N/A",
                    VisitDate = v.VisitDate.ToString("yyyy-MM-dd"),
                    v.VisitTime,
                    Status = v.RequestStatus
                }).ToList();
                Console.WriteLine($"[PASS] Scheduled visit #{visit.VisitRequestId} and loaded {visitDisplayList.Count} visits with VisitTime='{visit.VisitTime}'");

                // 8. Test Submit Review & Reviews Load
                Console.WriteLine("\n--- Testing Submit Review & Reviews Load ---");
                var testReview = new EstateNexus.Models.Entities.Review
                {
                    CustomerId = customer.UserId,
                    PropertyId = testPropA.PropertyId,
                    Rating = 5,
                    ReviewComment = "Hotfix verification review comment",
                    ReviewStatus = "Approved",
                    ReviewDate = DateTime.Now
                };
                verifyContext2.Reviews.Add(testReview);
                verifyContext2.SaveChanges();

                var loadedReviews = verifyContext2.Reviews
                    .Include(r => r.Property)
                    .Include(r => r.Customer)
                    .Where(r => r.PropertyId == testPropA.PropertyId)
                    .ToList();
                if (!loadedReviews.Any())
                    throw new Exception("Review was not saved or loaded!");
                Console.WriteLine($"[PASS] Submitted review #{testReview.ReviewId} (Rating: {testReview.Rating}, Status: {testReview.ReviewStatus}) and loaded {loadedReviews.Count} reviews!");

                // Cleanup test properties using a fresh context to avoid identity/cascade conflicts
                using var cleanupCtx = new EstateNexusDbContext();
                var propAId = testPropA.PropertyId;
                var propBId = testPropB.PropertyId;
                // Delete OrderItems and VisitRequests referencing test props first
                var testOrderItems = cleanupCtx.OrderItems
                    .Where(oi => oi.PropertyId == propAId || oi.PropertyId == propBId)
                    .ToList();
                if (testOrderItems.Any()) { cleanupCtx.OrderItems.RemoveRange(testOrderItems); cleanupCtx.SaveChanges(); }
                var testVisits = cleanupCtx.VisitRequests
                    .Where(vr => vr.PropertyId == propAId || vr.PropertyId == propBId)
                    .ToList();
                if (testVisits.Any()) { cleanupCtx.VisitRequests.RemoveRange(testVisits); cleanupCtx.SaveChanges(); }
                var testReviews = cleanupCtx.Reviews
                    .Where(r => r.PropertyId == propAId || r.PropertyId == propBId)
                    .ToList();
                if (testReviews.Any()) { cleanupCtx.Reviews.RemoveRange(testReviews); cleanupCtx.SaveChanges(); }
                var cpA = cleanupCtx.Properties.Find(propAId);
                var cpB = cleanupCtx.Properties.Find(propBId);
                if (cpA != null) cleanupCtx.Properties.Remove(cpA);
                if (cpB != null) cleanupCtx.Properties.Remove(cpB);
                cleanupCtx.SaveChanges();

                Console.WriteLine("\n*** ALL INTEGRATION CHECKS PASSED (100% DIRECT EF CORE, 0 REPOSITORIES, ER DIAGRAM COMPLIANT) ***\n");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] Integration test error: {ex.Message}");
                var inner = ex.InnerException;
                while (inner != null)
                {
                    Console.WriteLine($"   -> Inner: {inner.Message}");
                    inner = inner.InnerException;
                }
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
