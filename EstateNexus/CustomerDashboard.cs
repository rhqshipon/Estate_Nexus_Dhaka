using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class CustomerDashboard : Form
    {
        public CustomerDashboard()
        {
            InitializeComponent();
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Customer Dashboard - Welcome, " + Session.FullName;
            cmbListingTypeFilter.SelectedIndex = 0;
            LoadBrowseProperties("", "All");
            LoadCart();
            LoadOrders();
            LoadMyVisits();
            LoadReviews();
            LoadUserProfile();
        }

        private void LoadBrowseProperties(string searchTerm, string typeFilter)
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var query = context.Properties
                    .Include(p => p.Category)
                    .Include(p => p.Owner)
                    .Where(p => p.PropertyStatus == "Available");

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p => p.PropertyTitle.Contains(searchTerm) ||
                                             p.District.Contains(searchTerm) ||
                                             p.AreaLocation.Contains(searchTerm));
                }

                if (typeFilter != "All")
                {
                    query = query.Where(p => p.ListingType == typeFilter);
                }

                var list = query.Select(p => new
                {
                    p.PropertyId,
                    PropertyTitle = p.PropertyTitle,
                    Category = p.Category != null ? p.Category.CategoryName : "",
                    p.ListingType,
                    Location = p.District + ", " + p.AreaLocation,
                    Address = p.FullAddress,
                    AreaSize = p.AreaSize,
                    AreaUnit = p.AreaUnit,
                    p.Bedrooms,
                    p.Bathrooms,
                    p.Price,
                    Status = p.PropertyStatus,
                    Seller = p.Owner != null ? p.Owner.FullName : ""
                }).ToList();

                dgvBrowseProperties.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading properties: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            string type = cmbListingTypeFilter.SelectedItem?.ToString() ?? "All";
            LoadBrowseProperties(term, type);
        }

        private void LoadCart()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var cart = context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Property)
                    .FirstOrDefault(c => c.CustomerId == Session.UserId && c.IsActive);

                if (cart != null && cart.CartItems.Any())
                {
                    var items = cart.CartItems.Select(ci => new
                    {
                        ci.CartItemId,
                        ci.PropertyId,
                        PropertyTitle = ci.Property.PropertyTitle,
                        ListingType = ci.Property.ListingType,
                        RentalMonths = ci.RentalMonths,
                        UnitPrice = ci.Property.Price,
                        OfferedPrice = ci.OfferedPrice ?? (ci.Property.ListingType == "Rent" ? ci.Property.Price * (ci.RentalMonths > 0 ? ci.RentalMonths : 1) : ci.Property.Price),
                        Location = ci.Property.District + ", " + ci.Property.AreaLocation
                    }).ToList();

                    dgvCart.DataSource = items;

                    decimal total = items.Sum(i => i.OfferedPrice);
                    lblCartTotal.Text = "Total Amount: ৳" + total.ToString("N2");
                }
                else
                {
                    dgvCart.DataSource = null;
                    lblCartTotal.Text = "Total Amount: ৳0.00";
                }
            }
            catch
            {
                lblCartTotal.Text = "Total Amount: ৳0.00";
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvBrowseProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property from the table first.");
                return;
            }

            int propertyId = Convert.ToInt32(dgvBrowseProperties.SelectedRows[0].Cells["PropertyId"].Value);

            try
            {
                using var context = new EstateNexusDbContext();
                var property = context.Properties.Find(propertyId);
                if (property == null || property.PropertyStatus != "Available")
                {
                    MessageBox.Show("This property is no longer available.");
                    return;
                }

                // 1. Get or Create Cart for user
                var cart = context.Carts.FirstOrDefault(c => c.CustomerId == Session.UserId && c.IsActive);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CustomerId = Session.UserId,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }

                // 2. Check if already in cart
                bool alreadyInCart = context.CartItems.Any(ci => ci.CartId == cart.CartId && ci.PropertyId == propertyId);
                if (alreadyInCart)
                {
                    MessageBox.Show("This property is already in your cart.");
                    return;
                }

                // 3. Add to Cart Items
                int rentalMonths = 1;
                decimal offeredPrice = property.Price;
                if (property.ListingType == "Rent")
                {
                    rentalMonths = (int)numRentalMonths.Value;
                    offeredPrice = property.Price * rentalMonths;
                }

                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    PropertyId = propertyId,
                    RentalMonths = rentalMonths,
                    OfferedPrice = offeredPrice,
                    AddedDate = DateTime.Now
                };

                context.CartItems.Add(cartItem);
                context.SaveChanges();

                MessageBox.Show("Property added to cart successfully!");
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding to cart: " + ex.Message);
            }
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item from your cart to remove.");
                return;
            }

            int cartItemId = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["CartItemId"].Value);

            try
            {
                using var context = new EstateNexusDbContext();
                var item = context.CartItems.Find(cartItemId);
                if (item != null)
                {
                    context.CartItems.Remove(item);
                    context.SaveChanges();
                    MessageBox.Show("Item removed from cart.");
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing item: " + ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Do you want to proceed with Checkout & Payment?", "Confirm Purchase", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var context = new EstateNexusDbContext();
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var cart = context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Property)
                        .FirstOrDefault(c => c.CustomerId == Session.UserId && c.IsActive);

                    if (cart == null || !cart.CartItems.Any())
                    {
                        MessageBox.Show("Your cart is empty.");
                        return;
                    }

                    decimal totalAmount = cart.CartItems.Sum(ci => ci.OfferedPrice ?? (ci.Property.ListingType == "Rent" ? ci.Property.Price * (ci.RentalMonths > 0 ? ci.RentalMonths : 1) : ci.Property.Price));
                    string transactionType = cart.CartItems.Any(ci => ci.Property.ListingType == "Rent") ? "Rental" : "Sale";

                    // 1. Create Order
                    var order = new Order
                    {
                        CustomerId = Session.UserId,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        OrderStatus = "Completed",
                        TransactionType = transactionType
                    };
                    context.Orders.Add(order);
                    context.SaveChanges(); // generates order.OrderId

                    // 2. Create Payment
                    var payment = new Payment
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
                    context.SaveChanges(); // generates payment.PaymentId

                    // 3. Create OrderItems from CartItems & Commissions
                    decimal totalCommission = 0m;
                    foreach (var ci in cart.CartItems)
                    {
                        decimal itemFinal = ci.OfferedPrice ?? (ci.Property.ListingType == "Rent" ? ci.Property.Price * (ci.RentalMonths > 0 ? ci.RentalMonths : 1) : ci.Property.Price);

                        var orderItem = new OrderItem
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

                        // Platform commission (5%)
                        decimal commAmount = Math.Round(itemFinal * 0.05m, 2);
                        decimal ownerAmount = itemFinal - commAmount;
                        totalCommission += commAmount;

                        var commission = new Commission
                        {
                            OrderId = order.OrderId,
                            CommissionRate = 5.00m,
                            TransactionAmount = itemFinal,
                            CommissionAmount = commAmount,
                            OwnerAmount = ownerAmount,
                            CreatedDate = DateTime.Now
                        };
                        context.Commissions.Add(commission);

                        // Update Property status to Sold / Rented
                        ci.Property.PropertyStatus = ci.Property.ListingType == "Rent" ? "Rented" : "Sold";
                        ci.Property.UpdatedDate = DateTime.Now;
                    }

                    // 4. Create Invoice linked to both OrderId and PaymentId
                    var invoice = new Invoice
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

                    // 5. Clear Cart Items
                    context.CartItems.RemoveRange(cart.CartItems);

                    context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show(
                        $"Checkout Successful!\nOrder ID: #{order.OrderId}\nTransaction ID: {payment.TransactionId}\nInvoice Number: {invoice.InvoiceNumber}\nTotal Amount Paid: ৳{totalAmount:N2}",
                        "Invoice / Purchase Receipt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadCart();
                    LoadOrders();
                    LoadBrowseProperties("", cmbListingTypeFilter.SelectedItem?.ToString() ?? "All");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaction failed: " + ex.Message, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during checkout: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabBrowse)
            {
                string term = txtSearch.Text.Trim();
                string type = cmbListingTypeFilter.SelectedItem?.ToString() ?? "All";
                LoadBrowseProperties(term, type);
            }
            else if (tabControl1.SelectedTab == tabCart)
            {
                LoadCart();
            }
            else if (tabControl1.SelectedTab == tabOrders)
            {
                LoadOrders();
            }
            else if (tabControl1.SelectedTab == tabMyVisits)
            {
                LoadMyVisits();
            }
            else if (tabControl1.SelectedTab == tabReviews)
            {
                LoadReviews();
            }
            else if (tabControl1.SelectedTab == tabProfile)
            {
                LoadUserProfile();
            }
        }

        private void btnRequestVisit_Click(object sender, EventArgs e)
        {
            if (dgvBrowseProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property from the list first.", "No Property Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int propertyId = Convert.ToInt32(dgvBrowseProperties.SelectedRows[0].Cells["PropertyId"].Value);
            string propName = dgvBrowseProperties.SelectedRows[0].Cells["PropertyTitle"].Value?.ToString() ?? "Property";
            string propLocation = dgvBrowseProperties.SelectedRows[0].Cells["Location"].Value?.ToString() ?? "";

            using var scheduleForm = new ScheduleVisitForm(propertyId, propName, propLocation);
            if (scheduleForm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    using var context = new EstateNexusDbContext();

                    // Check for existing pending request
                    bool alreadyPending = context.VisitRequests.Any(v =>
                        v.CustomerId == Session.UserId &&
                        v.PropertyId == propertyId &&
                        v.RequestStatus == "Pending");

                    if (alreadyPending)
                    {
                        var confirm = MessageBox.Show(
                            $"You already have a pending visit request for '{propName}'.\nDo you still want to schedule another visit?",
                            "Existing Pending Request",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirm != DialogResult.Yes)
                            return;
                    }

                    var visit = new VisitRequest
                    {
                        CustomerId = Session.UserId,
                        PropertyId = propertyId,
                        VisitDate = scheduleForm.SelectedDate,
                        VisitTime = scheduleForm.SelectedTime,
                        RequestStatus = "Pending",
                        CustomerNote = string.IsNullOrWhiteSpace(scheduleForm.CustomerNote) ? "Scheduled via customer portal" : scheduleForm.CustomerNote,
                        CreatedDate = DateTime.Now
                    };

                    context.VisitRequests.Add(visit);
                    context.SaveChanges();

                    MessageBox.Show(
                        $"Visit request scheduled successfully!\n\nProperty: {propName}\nDate: {visit.VisitDate:yyyy-MM-dd}\nTime: {visit.VisitTime}\nStatus: Pending Seller Approval",
                        "Visit Request Submitted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadMyVisits();
                    tabControl1.SelectedTab = tabMyVisits;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error requesting visit: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelVisit_Click(object sender, EventArgs e)
        {
            if (dgvMyVisits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a visit request from the table to cancel.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int visitId = Convert.ToInt32(dgvMyVisits.SelectedRows[0].Cells["VisitId"].Value);
            string currentStatus = dgvMyVisits.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";

            if (currentStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This visit request is already cancelled.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This visit request was already rejected by the seller.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to cancel this visit request?",
                "Confirm Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var context = new EstateNexusDbContext();
                    var visit = context.VisitRequests.FirstOrDefault(v => v.VisitRequestId == visitId && v.CustomerId == Session.UserId);
                    if (visit != null)
                    {
                        visit.RequestStatus = "Cancelled";
                        context.SaveChanges();
                        MessageBox.Show("Visit request has been cancelled.", "Request Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMyVisits();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error cancelling visit request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefreshVisits_Click(object sender, EventArgs e)
        {
            LoadMyVisits();
        }

        private void LoadOrders()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var orders = context.Orders
                    .Where(o => o.CustomerId == Session.UserId)
                    .Include(o => o.Payments)
                    .Include(o => o.Invoice)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Property)
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.OrderDate,
                        o.TotalAmount,
                        PaymentMethod = o.Payments.Any() ? o.Payments.First().PaymentMethod : "Online/Card",
                        Status = o.OrderStatus,
                        Properties = string.Join(", ", o.OrderItems.Select(oi => oi.Property.PropertyTitle)),
                        InvoiceNo = o.Invoice != null ? o.Invoice.InvoiceNumber : "N/A"
                    })
                    .ToList();

                dgvOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private void LoadMyVisits()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var rawVisits = context.VisitRequests
                    .Where(v => v.CustomerId == Session.UserId)
                    .Include(v => v.Property)
                    .ThenInclude(p => p.Owner)
                    .OrderByDescending(v => v.CreatedDate)
                    .ToList();

                var displayList = rawVisits.Select(v => new
                {
                    VisitId = v.VisitRequestId,
                    PropertyTitle = v.Property != null ? v.Property.PropertyTitle : "N/A",
                    Location = v.Property != null ? (v.Property.District + ", " + v.Property.AreaLocation) : "N/A",
                    Seller = (v.Property != null && v.Property.Owner != null) ? v.Property.Owner.FullName : "N/A",
                    SellerPhone = (v.Property != null && v.Property.Owner != null) ? (v.Property.Owner.Phone ?? "N/A") : "N/A",
                    VisitDate = v.VisitDate.ToString("yyyy-MM-dd"),
                    v.VisitTime,
                    Status = v.RequestStatus,
                    CustomerNote = v.CustomerNote ?? ""
                }).ToList();

                dgvMyVisits.DataSource = displayList;

                int total = displayList.Count;
                int pending = displayList.Count(v => v.Status == "Pending");
                int approved = displayList.Count(v => v.Status == "Approved");
                int rejected = displayList.Count(v => v.Status == "Rejected");
                int cancelled = displayList.Count(v => v.Status == "Cancelled");
                lblVisitStatusSummary.Text = $"Total: {total} | Pending: {pending} | Approved: {approved} | Rejected: {rejected} | Cancelled: {cancelled}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading visits: " + ex.Message);
            }
        }

        private void LoadReviews()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var reviews = context.Reviews
                    .Include(r => r.Property)
                    .Include(r => r.Customer)
                    .OrderByDescending(r => r.ReviewDate)
                    .Select(r => new
                    {
                        r.ReviewId,
                        PropertyTitle = r.Property.PropertyTitle,
                        Customer = r.Customer != null ? r.Customer.FullName : "",
                        r.Rating,
                        Comment = r.ReviewComment,
                        r.ReviewDate
                    })
                    .ToList();

                dgvReviews.DataSource = reviews;

                var propList = context.Properties
                    .Select(p => new
                    {
                        p.PropertyId,
                        p.PropertyTitle
                    })
                    .ToList();

                cmbReviewProperty.DisplayMember = "PropertyTitle";
                cmbReviewProperty.ValueMember = "PropertyId";
                cmbReviewProperty.DataSource = propList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reviews: " + ex.Message);
            }
        }

        private void btnSubmitReview_Click(object sender, EventArgs e)
        {
            if (cmbReviewProperty.SelectedValue == null)
            {
                MessageBox.Show("Please select a property from the dropdown to leave a review.");
                return;
            }

            int propId = Convert.ToInt32(cmbReviewProperty.SelectedValue);
            string propName = cmbReviewProperty.Text;
            int rating = (int)numRating.Value;
            string comment = txtReviewComment.Text.Trim();

            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please write a short comment.");
                return;
            }

            try
            {
                using var context = new EstateNexusDbContext();
                var review = new Review
                {
                    CustomerId = Session.UserId,
                    PropertyId = propId,
                    Rating = rating,
                    ReviewComment = comment,
                    ReviewStatus = "Approved",
                    ReviewDate = DateTime.Now
                };

                context.Reviews.Add(review);
                context.SaveChanges();

                MessageBox.Show($"Review submitted for '{propName}'!");
                txtReviewComment.Clear();
                LoadReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting review: " + ex.Message);
            }
        }

        private void LoadUserProfile()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var user = context.Users.Find(Session.UserId);
                if (user != null)
                {
                    txtProfileFullName.Text = user.FullName;
                    txtProfileEmail.Text = user.Email;
                    txtProfilePhone.Text = user.Phone;
                    txtProfileAddress.Text = user.Address;
                    txtProfileImagePath.Text = user.ProfileImagePath;

                    if (!string.IsNullOrEmpty(user.ProfileImagePath) && File.Exists(user.ProfileImagePath))
                    {
                        try { picProfilePreview.ImageLocation = user.ProfileImagePath; } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message);
            }
        }

        private void btnBrowseProfileImage_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            ofd.Title = "Select Profile Image";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtProfileImagePath.Text = ofd.FileName;
                picProfilePreview.ImageLocation = ofd.FileName;
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var user = context.Users.Find(Session.UserId);
                if (user != null)
                {
                    user.FullName = txtProfileFullName.Text.Trim();
                    user.Phone = txtProfilePhone.Text.Trim();
                    user.Address = txtProfileAddress.Text.Trim();
                    user.ProfileImagePath = txtProfileImagePath.Text.Trim();

                    context.SaveChanges();

                    Session.FullName = user.FullName;
                    Session.ProfileImagePath = user.ProfileImagePath;
                    lblTitle.Text = "Customer Dashboard - Welcome, " + Session.FullName;

                    MessageBox.Show("Profile settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Logout();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
