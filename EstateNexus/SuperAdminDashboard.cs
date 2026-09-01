using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace EstateNexus
{
    public partial class SuperAdminDashboard : Form
    {
        public SuperAdminDashboard()
        {
            InitializeComponent();
        }

        private void SuperAdminDashboard_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Super Admin Dashboard - " + Session.FullName;
            LoadUsers();
            LoadProperties();
            LoadRevenue();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT UserId, FullName, Email, Phone, Role, AccountStatus, CreatedAt FROM Users", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void LoadProperties()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT p.PropertyId, u.FullName as Owner, p.PropertyName, c.CategoryName, p.ListingType, p.Location, p.Price, p.Status 
                        FROM Properties p
                        LEFT JOIN Users u ON p.OwnerId = u.UserId
                        LEFT JOIN PropertyCategories c ON p.CategoryId = c.CategoryId";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProperties.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading properties: " + ex.Message);
            }
        }

        private void LoadRevenue()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT o.OrderId, u.FullName as Customer, o.TotalAmount, o.PaymentMethod, o.OrderDate, o.Status 
                        FROM Orders o
                        JOIN Users u ON o.CustomerId = u.UserId
                        ORDER BY o.OrderDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAllOrders.DataSource = dt;

                    decimal totalVol = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalVol += Convert.ToDecimal(row["TotalAmount"]);
                    }
                    decimal commission = totalVol * 0.05m; // 5% platform commission

                    lblRevenue.Text = "Total Marketplace Volume: ৳" + totalVol.ToString("N2");
                    lblCommission.Text = "Platform Commission (5%): ৳" + commission.ToString("N2");
                }
            }
            catch
            {
                lblRevenue.Text = "Total Marketplace Volume: ৳0.00";
                lblCommission.Text = "Platform Commission (5%): ৳0.00";
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to toggle status.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);
            string currentStatus = dgvUsers.SelectedRows[0].Cells["AccountStatus"].Value?.ToString() ?? "Active";
            string newStatus = currentStatus == "Active" ? "Inactive" : "Active";

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "UPDATE Users SET AccountStatus = @Status WHERE UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User status changed to: " + newStatus);
                        LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user status: " + ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

            if (userId == Session.UserId)
            {
                MessageBox.Show("You cannot delete your own Super Admin account!");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this user? This will also remove their associated data.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                    {
                        string query = "DELETE FROM Users WHERE UserId = @UserId";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("User deleted successfully.");
                            LoadUsers();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message);
                }
            }
        }

        private void btnDeleteProperty_Click(object sender, EventArgs e)
        {
            if (dgvProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property to remove.");
                return;
            }

            int propId = Convert.ToInt32(dgvProperties.SelectedRows[0].Cells["PropertyId"].Value);

            DialogResult confirm = MessageBox.Show("Are you sure you want to remove this property?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                    {
                        string query = "DELETE FROM Properties WHERE PropertyId = @PropertyId";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@PropertyId", propId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Property removed from platform.");
                            LoadProperties();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing property: " + ex.Message);
                }
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
