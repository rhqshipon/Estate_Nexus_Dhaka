using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace EstateNexus
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Seller Dashboard - " + Session.FullName;
            LoadMyProperties();
            LoadVisitRequests();
            LoadSales();
        }

        private void LoadMyProperties()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT p.PropertyId, p.PropertyName, c.CategoryName, p.ListingType, p.Location, p.Price, p.Status 
                        FROM Properties p
                        LEFT JOIN PropertyCategories c ON p.CategoryId = c.CategoryId
                        WHERE p.OwnerId = @OwnerId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvMyProperties.DataSource = dt;

                        // Calculate inventory stats
                        int total = dt.Rows.Count;
                        int available = 0;
                        int sold = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            string status = row["Status"]?.ToString() ?? "";
                            if (status == "Available") available++;
                            else sold++;
                        }
                        lblPropertyStats.Text = $"Total: {total} | Available: {available} | Sold: {sold}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading properties: " + ex.Message);
            }
        }

        private void LoadVisitRequests()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT v.VisitId, u.FullName as Customer, u.Phone, p.PropertyName, v.VisitDate, v.VisitTime, v.Status 
                        FROM VisitRequests v
                        JOIN Properties p ON v.PropertyId = p.PropertyId
                        JOIN Users u ON v.CustomerId = u.UserId
                        WHERE p.OwnerId = @OwnerId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvVisitRequests.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading visit requests: " + ex.Message);
            }
        }

        private void LoadSales()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        SELECT o.OrderId, u.FullName as CustomerName, p.PropertyName, oi.FinalAmount as Amount, o.OrderDate 
                        FROM OrderItems oi
                        JOIN Orders o ON oi.OrderId = o.OrderId
                        JOIN Properties p ON oi.PropertyId = p.PropertyId
                        JOIN Users u ON o.CustomerId = u.UserId
                        WHERE p.OwnerId = @OwnerId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvSales.DataSource = dt;

                        decimal totalEarnings = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            totalEarnings += Convert.ToDecimal(row["Amount"]);
                        }
                        lblTotalEarnings.Text = "Total Earnings from Sales: ৳" + totalEarnings.ToString("N2");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales: " + ex.Message);
            }
        }

        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            AddPropertyForm addForm = new AddPropertyForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadMyProperties();
            }
        }

        private void btnDeleteProperty_Click(object sender, EventArgs e)
        {
            if (dgvMyProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property to delete.");
                return;
            }

            int propId = Convert.ToInt32(dgvMyProperties.SelectedRows[0].Cells["PropertyId"].Value);

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this property?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                    {
                        string query = "DELETE FROM Properties WHERE PropertyId = @PropertyId AND OwnerId = @OwnerId";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@PropertyId", propId);
                            cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Property deleted successfully.");
                            LoadMyProperties();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting property: " + ex.Message);
                }
            }
        }

        private void btnMarkSold_Click(object sender, EventArgs e)
        {
            if (dgvMyProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a property.");
                return;
            }

            int propId = Convert.ToInt32(dgvMyProperties.SelectedRows[0].Cells["PropertyId"].Value);
            string currentStatus = dgvMyProperties.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "Available";
            string newStatus = currentStatus == "Available" ? "Sold" : "Available";

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "UPDATE Properties SET Status = @Status WHERE PropertyId = @PropertyId AND OwnerId = @OwnerId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@PropertyId", propId);
                        cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Property status updated to: " + newStatus);
                        LoadMyProperties();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message);
            }
        }

        private void btnApproveVisit_Click(object sender, EventArgs e)
        {
            UpdateVisitStatus("Approved");
        }

        private void btnRejectVisit_Click(object sender, EventArgs e)
        {
            UpdateVisitStatus("Rejected");
        }

        private void UpdateVisitStatus(string newStatus)
        {
            if (dgvVisitRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a visit request.");
                return;
            }

            int visitId = Convert.ToInt32(dgvVisitRequests.SelectedRows[0].Cells["VisitId"].Value);

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "UPDATE VisitRequests SET Status = @Status WHERE VisitId = @VisitId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@VisitId", visitId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Visit request marked as: " + newStatus);
                        LoadVisitRequests();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating visit request: " + ex.Message);
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
