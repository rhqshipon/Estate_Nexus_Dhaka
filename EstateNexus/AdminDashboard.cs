using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

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
                using (var context = new EstateNexusDbContext())
                {
                    var list = context.Properties
                        .Where(p => p.OwnerId == Session.UserId)
                        .Include(p => p.Category)
                        .Select(p => new
                        {
                            p.PropertyId,
                            p.PropertyTitle,
                            Category = p.Category != null ? p.Category.CategoryName : "",
                            p.ListingType,
                            Location = p.District + ", " + p.AreaLocation,
                            p.Price,
                            Status = p.PropertyStatus
                        })
                        .ToList();

                    dgvMyProperties.DataSource = list;

                    int total = list.Count;
                    int available = list.Count(p => p.Status == "Available");
                    int sold = total - available;
                    lblPropertyStats.Text = $"Total: {total} | Available: {available} | Sold: {sold}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading properties: " + ex.Message);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabMyProperties)
            {
                LoadMyProperties();
            }
            else if (tabControl1.SelectedTab == tabVisitRequests)
            {
                LoadVisitRequests();
            }
            else if (tabControl1.SelectedTab == tabSales)
            {
                LoadSales();
            }
        }

        private void LoadVisitRequests()
        {
            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var query = context.VisitRequests
                        .Include(v => v.Property)
                        .Include(v => v.Customer)
                        .Where(v => v.Property != null && v.Property.OwnerId == Session.UserId);

                    var rawList = query.ToList();

                    int total = rawList.Count;
                    int pending = rawList.Count(v => v.RequestStatus == "Pending");
                    int approved = rawList.Count(v => v.RequestStatus == "Approved");
                    int rejected = rawList.Count(v => v.RequestStatus == "Rejected");
                    int cancelled = rawList.Count(v => v.RequestStatus == "Cancelled");

                    lblVisitStats.Text = $"Total: {total} | Pending: {pending} | Approved: {approved} | Rejected: {rejected} | Cancelled: {cancelled}";

                    string filter = cmbVisitFilter.SelectedItem?.ToString() ?? "All";
                    if (filter != "All")
                    {
                        rawList = rawList.Where(v => v.RequestStatus.Equals(filter, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    var visits = rawList
                        .OrderBy(v => v.RequestStatus == "Pending" ? 0 : 1)
                        .ThenByDescending(v => v.CreatedDate)
                        .Select(v => new
                        {
                            VisitId = v.VisitRequestId,
                            Customer = v.Customer != null ? v.Customer.FullName : "N/A",
                            Phone = v.Customer != null ? (v.Customer.Phone ?? "N/A") : "N/A",
                            Email = v.Customer != null ? (v.Customer.Email ?? "N/A") : "N/A",
                            PropertyTitle = v.Property != null ? v.Property.PropertyTitle : "N/A",
                            VisitDate = v.VisitDate.ToString("yyyy-MM-dd"),
                            v.VisitTime,
                            Status = v.RequestStatus,
                            CustomerNote = v.CustomerNote ?? "",
                            RequestedOn = v.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                        })
                        .ToList();

                    dgvVisitRequests.DataSource = visits;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading visit requests: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbVisitFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVisitRequests();
        }

        private void btnRefreshVisitRequests_Click(object sender, EventArgs e)
        {
            LoadVisitRequests();
        }

        private void LoadSales()
        {
            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var sales = context.OrderItems
                        .Include(oi => oi.Order).ThenInclude(o => o.Customer)
                        .Include(oi => oi.Property)
                        .Where(oi => oi.OwnerId == Session.UserId)
                        .Select(oi => new
                        {
                            oi.OrderId,
                            CustomerName = oi.Order.Customer != null ? oi.Order.Customer.FullName : "",
                            PropertyTitle = oi.Property != null ? oi.Property.PropertyTitle : "",
                            Amount = oi.FinalAmount,
                            oi.Order.OrderDate
                        })
                        .ToList();

                    dgvSales.DataSource = sales;

                    decimal totalEarnings = sales.Sum(s => s.Amount);
                    lblTotalEarnings.Text = "Total Earnings from Sales: ৳" + totalEarnings.ToString("N2");
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
                    using (var context = new EstateNexusDbContext())
                    {
                        var prop = context.Properties.FirstOrDefault(p => p.PropertyId == propId && p.OwnerId == Session.UserId);
                        if (prop != null)
                        {
                            context.Properties.Remove(prop);
                            context.SaveChanges();
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

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var prop = context.Properties.FirstOrDefault(p => p.PropertyId == propId && p.OwnerId == Session.UserId);
                    if (prop != null)
                    {
                        prop.PropertyStatus = prop.PropertyStatus == "Available" ? "Sold" : "Available";
                        context.SaveChanges();
                        MessageBox.Show("Property status updated to: " + prop.PropertyStatus);
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
                MessageBox.Show("Please select a visit request from the list.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int visitId = Convert.ToInt32(dgvVisitRequests.SelectedRows[0].Cells["VisitId"].Value);
            string currentStatus = dgvVisitRequests.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";

            if (currentStatus.Equals(newStatus, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"This visit request is already marked as {newStatus}.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                var prompt = MessageBox.Show(
                    $"This visit request was cancelled by the customer.\nDo you still want to change its status to '{newStatus}'?",
                    "Customer Cancelled Request",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (prompt != DialogResult.Yes)
                    return;
            }

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var visit = context.VisitRequests.Find(visitId);
                    if (visit != null)
                    {
                        visit.RequestStatus = newStatus;
                        context.SaveChanges();
                        MessageBox.Show($"Visit request marked as: {newStatus}", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVisitRequests();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating visit request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
