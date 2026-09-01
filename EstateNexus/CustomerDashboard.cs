using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

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
        }

        private void LoadBrowseProperties(string searchTerm, string typeFilter)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT p.PropertyId, p.PropertyName, c.CategoryName, p.ListingType, p.Location, p.Address, p.Area as [Area (sqft)], p.Bedrooms, p.Bathrooms, p.Price, p.Status, u.FullName as Seller 
                        FROM Properties p
                        LEFT JOIN PropertyCategories c ON p.CategoryId = c.CategoryId
                        LEFT JOIN Users u ON p.OwnerId = u.UserId
                        WHERE p.Status = 'Available' 
                          AND (p.PropertyName LIKE @Search OR p.Location LIKE @Search)";

                    if (typeFilter != "All")
                    {
                        query += " AND p.ListingType = @ListingType";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");
                        if (typeFilter != "All")
                        {
                            cmd.Parameters.AddWithValue("@ListingType", typeFilter);
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvBrowseProperties.DataSource = dt;
                    }
                }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT c.CartItemId, p.PropertyId, p.PropertyName, p.ListingType, p.Location, p.Price 
                        FROM ReservationCartItems c 
                        JOIN Properties p ON c.PropertyId = p.PropertyId 
                        JOIN ReservationCart rc ON c.CartId = rc.CartId 
                        WHERE rc.CustomerId = @CustomerId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvCart.DataSource = dt;

                        decimal total = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            total += Convert.ToDecimal(row["Price"]);
                        }
                        lblCartTotal.Text = "Total Amount: ৳" + total.ToString("N2");
                    }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    con.Open();

                    // 1. Get or Create Cart for user
                    int cartId = 0;
                    string checkCartQuery = "SELECT CartId FROM ReservationCart WHERE CustomerId = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(checkCartQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            cartId = Convert.ToInt32(result);
                        }
                        else
                        {
                            string createCartQuery = "INSERT INTO ReservationCart (CustomerId, CreatedAt) OUTPUT INSERTED.CartId VALUES (@CustomerId, GETDATE())";
                            using (SqlCommand createCmd = new SqlCommand(createCartQuery, con))
                            {
                                createCmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                                cartId = (int)createCmd.ExecuteScalar();
                            }
                        }
                    }

                    // 2. Check if already in cart
                    string checkItemQuery = "SELECT COUNT(*) FROM ReservationCartItems WHERE CartId = @CartId AND PropertyId = @PropertyId";
                    using (SqlCommand checkItemCmd = new SqlCommand(checkItemQuery, con))
                    {
                        checkItemCmd.Parameters.AddWithValue("@CartId", cartId);
                        checkItemCmd.Parameters.AddWithValue("@PropertyId", propertyId);
                        int count = (int)checkItemCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("This property is already in your cart.");
                            return;
                        }
                    }

                    // 3. Add to Cart Items
                    string insertItemQuery = "INSERT INTO ReservationCartItems (CartId, PropertyId, AddedAt) VALUES (@CartId, @PropertyId, GETDATE())";
                    using (SqlCommand insertCmd = new SqlCommand(insertItemQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("@CartId", cartId);
                        insertCmd.Parameters.AddWithValue("@PropertyId", propertyId);
                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Property added to cart successfully!");
                    LoadCart();
                }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "DELETE FROM ReservationCartItems WHERE CartItemId = @CartItemId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item removed from cart.");
                        LoadCart();
                    }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // Calculate total
                            decimal total = 0;
                            foreach (DataGridViewRow row in dgvCart.Rows)
                            {
                                total += Convert.ToDecimal(row.Cells["Price"].Value);
                            }

                            // 1. Create Order
                            string createOrderQuery = @"
                                INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, PaymentMethod, Status) 
                                OUTPUT INSERTED.OrderId 
                                VALUES (@CustomerId, GETDATE(), @TotalAmount, 'Online/Card', 'Completed')";

                            int orderId = 0;
                            using (SqlCommand cmd = new SqlCommand(createOrderQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                                cmd.Parameters.AddWithValue("@TotalAmount", total);
                                orderId = (int)cmd.ExecuteScalar();
                            }

                            // 2. Add OrderItems & Mark Properties as Sold
                            foreach (DataGridViewRow row in dgvCart.Rows)
                            {
                                int propId = Convert.ToInt32(row.Cells["PropertyId"].Value);
                                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                                string insertOrderItem = @"
                                    INSERT INTO OrderItems (OrderId, PropertyId, UnitPrice, FinalAmount) 
                                    VALUES (@OrderId, @PropertyId, @UnitPrice, @FinalAmount)";

                                using (SqlCommand itemCmd = new SqlCommand(insertOrderItem, con, transaction))
                                {
                                    itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                                    itemCmd.Parameters.AddWithValue("@PropertyId", propId);
                                    itemCmd.Parameters.AddWithValue("@UnitPrice", price);
                                    itemCmd.Parameters.AddWithValue("@FinalAmount", price);
                                    itemCmd.ExecuteNonQuery();
                                }

                                // Mark property as Sold
                                string updateProp = "UPDATE Properties SET Status = 'Sold' WHERE PropertyId = @PropertyId";
                                using (SqlCommand propCmd = new SqlCommand(updateProp, con, transaction))
                                {
                                    propCmd.Parameters.AddWithValue("@PropertyId", propId);
                                    propCmd.ExecuteNonQuery();
                                }
                            }

                            // 3. Clear Cart
                            string clearCart = @"
                                DELETE c FROM ReservationCartItems c 
                                JOIN ReservationCart rc ON c.CartId = rc.CartId 
                                WHERE rc.CustomerId = @CustomerId";

                            using (SqlCommand clearCmd = new SqlCommand(clearCart, con, transaction))
                            {
                                clearCmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                                clearCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show($"Checkout Successful!\nOrder ID: #{orderId}\nTotal Amount Paid: ৳{total:N2}\nInvoice Generated!", "Invoice / Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadCart();
                            LoadOrders();
                            LoadBrowseProperties("", cmbListingTypeFilter.SelectedItem?.ToString() ?? "All");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Transaction failed: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during checkout: " + ex.Message);
            }
        }

        private void btnRequestVisit_Click(object sender, EventArgs e)
        {
            if (dgvBrowseProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property first.");
                return;
            }

            int propertyId = Convert.ToInt32(dgvBrowseProperties.SelectedRows[0].Cells["PropertyId"].Value);
            string propName = dgvBrowseProperties.SelectedRows[0].Cells["PropertyName"].Value.ToString();

            // Simple visit schedule date (3 days from now)
            DateTime visitDate = DateTime.Today.AddDays(3);
            string visitTime = "04:00 PM";

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        INSERT INTO VisitRequests (CustomerId, PropertyId, VisitDate, VisitTime, Status, CreatedAt)
                        VALUES (@CustomerId, @PropertyId, @VisitDate, @VisitTime, 'Pending', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        cmd.Parameters.AddWithValue("@PropertyId", propertyId);
                        cmd.Parameters.AddWithValue("@VisitDate", visitDate);
                        cmd.Parameters.AddWithValue("@VisitTime", visitTime);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Visit request scheduled for:\nProperty: {propName}\nDate: {visitDate:yyyy-MM-dd}\nTime: {visitTime}\nStatus: Pending seller approval.", "Visit Scheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMyVisits();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error requesting visit: " + ex.Message);
            }
        }

        private void LoadOrders()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT o.OrderId, o.OrderDate, o.TotalAmount, o.PaymentMethod, o.Status, p.PropertyName, oi.FinalAmount 
                        FROM Orders o
                        JOIN OrderItems oi ON o.OrderId = oi.OrderId
                        JOIN Properties p ON oi.PropertyId = p.PropertyId
                        WHERE o.CustomerId = @CustomerId
                        ORDER BY o.OrderDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvOrders.DataSource = dt;
                    }
                }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT v.VisitId, p.PropertyName, p.Location, v.VisitDate, v.VisitTime, v.Status 
                        FROM VisitRequests v
                        JOIN Properties p ON v.PropertyId = p.PropertyId
                        WHERE v.CustomerId = @CustomerId
                        ORDER BY v.CreatedAt DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvMyVisits.DataSource = dt;
                    }
                }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT r.ReviewId, p.PropertyName, u.FullName as Customer, r.Rating, r.Comment, r.ReviewDate 
                        FROM Reviews r
                        JOIN Properties p ON r.PropertyId = p.PropertyId
                        JOIN Users u ON r.CustomerId = u.UserId
                        ORDER BY r.ReviewDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvReviews.DataSource = dt;

                    // Populate property dropdown for reviews
                    string propQuery = "SELECT PropertyId, PropertyName FROM Properties";
                    SqlDataAdapter propDa = new SqlDataAdapter(propQuery, con);
                    DataTable propDt = new DataTable();
                    propDa.Fill(propDt);
                    cmbReviewProperty.DisplayMember = "PropertyName";
                    cmbReviewProperty.ValueMember = "PropertyId";
                    cmbReviewProperty.DataSource = propDt;
                }
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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        INSERT INTO Reviews (CustomerId, PropertyId, Rating, Comment, ReviewDate)
                        VALUES (@CustomerId, @PropertyId, @Rating, @Comment, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", Session.UserId);
                        cmd.Parameters.AddWithValue("@PropertyId", propId);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Comment", comment);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Review submitted for '{propName}'!");
                        txtReviewComment.Clear();
                        LoadReviews();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting review: " + ex.Message);
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
