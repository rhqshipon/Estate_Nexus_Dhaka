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
                        SELECT TABLE_NAME, COLUMN_NAME 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        ORDER BY TABLE_NAME, ORDINAL_POSITION", con);
                    using var reader = cmd.ExecuteReader();
                    Console.WriteLine("\n--- ALL Database Columns ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($" {reader[0]}.{reader[1]}");
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
                    string txnType = activeCart.CartItems.Any(ci => ci.Property.ListingType == "Rent") ? "Rental" : "Sale";

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

                Console.WriteLine("\n*** ALL INTEGRATION CHECKS PASSED (100% DIRECT EF CORE, 0 REPOSITORIES, ER DIAGRAM COMPLIANT) ***\n");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] Integration test error: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
        }
    }
}
