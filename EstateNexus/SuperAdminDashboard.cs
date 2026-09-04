using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

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
                using var context = new EstateNexusDbContext();
                var users = context.Users
                    .Include(u => u.Role)
                    .Select(u => new
                    {
                        u.UserId,
                        u.FullName,
                        u.Email,
                        u.Phone,
                        Role = u.Role != null ? u.Role.RoleName : "",
                        u.AccountStatus,
                        u.IsActive,
                        u.CreatedDate
                    })
                    .ToList();

                dgvUsers.DataSource = users;
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
                using var context = new EstateNexusDbContext();
                var properties = context.Properties
                    .Include(p => p.Owner)
                    .Include(p => p.Category)
                    .Select(p => new
                    {
                        p.PropertyId,
                        Owner = p.Owner != null ? p.Owner.FullName : "",
                        PropertyTitle = p.PropertyTitle,
                        CategoryName = p.Category != null ? p.Category.CategoryName : "",
                        p.ListingType,
                        Location = p.District + ", " + p.AreaLocation,
                        p.Price,
                        Status = p.PropertyStatus
                    })
                    .ToList();

                dgvProperties.DataSource = properties;
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
                using var context = new EstateNexusDbContext();
                var orders = context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Payments)
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new
                    {
                        o.OrderId,
                        Customer = o.Customer != null ? o.Customer.FullName : "",
                        o.TotalAmount,
                        PaymentMethod = o.Payments.Any() ? o.Payments.First().PaymentMethod : "Card",
                        o.OrderDate,
                        Status = o.OrderStatus
                    })
                    .ToList();

                dgvAllOrders.DataSource = orders;

                decimal totalVol = orders.Sum(o => o.TotalAmount);
                decimal commission = Math.Round(totalVol * 0.05m, 2);

                lblRevenue.Text = "Total Marketplace Volume: ৳" + totalVol.ToString("N2");
                lblCommission.Text = "Platform Commission (5%): ৳" + commission.ToString("N2");
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

            if (userId == Session.UserId)
            {
                MessageBox.Show("You cannot deactivate or change the status of your own Super Admin account!", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new EstateNexusDbContext();
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    user.AccountStatus = user.AccountStatus == "Active" ? "Inactive" : "Active";
                    user.IsActive = user.AccountStatus == "Active";
                    context.SaveChanges();
                    MessageBox.Show("User status changed to: " + user.AccountStatus);
                    LoadUsers();
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
                    using var context = new EstateNexusDbContext();
                    var user = context.Users.Find(userId);
                    if (user != null)
                    {
                        context.Users.Remove(user);
                        context.SaveChanges();
                        MessageBox.Show("User deleted successfully.");
                        LoadUsers();
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
                    using var context = new EstateNexusDbContext();
                    var prop = context.Properties.Find(propId);
                    if (prop != null)
                    {
                        context.Properties.Remove(prop);
                        context.SaveChanges();
                        MessageBox.Show("Property removed from platform.");
                        LoadProperties();
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
