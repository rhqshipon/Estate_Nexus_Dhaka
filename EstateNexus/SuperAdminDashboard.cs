using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;

namespace EstateNexus
{
    public partial class SuperAdminDashboard : Form
    {
        public SuperAdminDashboard()
        {
            InitializeComponent();
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        private void SuperAdminDashboard_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Super Admin Dashboard - " + Session.FullName;

            LoadUsers();
            LoadProperties();
            LoadRevenue();
        }


        // ==========================================
        // LOAD USERS
        // ==========================================

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

                        Role = u.Role != null
                            ? u.Role.RoleName
                            : "",

                        u.AccountStatus,
                        u.IsActive,
                        u.CreatedDate
                    })
                    .ToList();

                dgvUsers.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading users:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // LOAD PROPERTIES
        // ==========================================

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

                        Owner = p.Owner != null
                            ? p.Owner.FullName
                            : "",

                        PropertyTitle = p.PropertyTitle,

                        CategoryName = p.Category != null
                            ? p.Category.CategoryName
                            : "",

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
                MessageBox.Show(
                    "Error loading properties:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // LOAD REVENUE AND ORDERS
        // ==========================================

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

                        Customer = o.Customer != null
                            ? o.Customer.FullName
                            : "",

                        o.TotalAmount,

                        PaymentMethod = o.Payments.Any()
                            ? o.Payments.First().PaymentMethod
                            : "N/A",

                        o.OrderDate,

                        Status = o.OrderStatus
                    })
                    .ToList();

                dgvAllOrders.DataSource = orders;

                // Calculate total marketplace volume
                decimal totalVolume = 0;

                foreach (var order in orders)
                {
                    totalVolume += order.TotalAmount;
                }

                // 5% platform commission
                decimal commission = totalVolume * 0.05m;

                lblRevenue.Text =
                    "Total Marketplace Volume: ৳" +
                    totalVolume.ToString("N2");

                lblCommission.Text =
                    "Platform Commission (5%): ৳" +
                    commission.ToString("N2");
            }
            catch (Exception ex)
            {
                dgvAllOrders.DataSource = null;

                lblRevenue.Text =
                    "Total Marketplace Volume: ৳0.00";

                lblCommission.Text =
                    "Platform Commission (5%): ৳0.00";

                MessageBox.Show(
                    "Error loading revenue:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // TOGGLE USER ACTIVE / INACTIVE
        // ==========================================

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a user first."
                );

                return;
            }


            int userId = Convert.ToInt32(
                dgvUsers.SelectedRows[0]
                    .Cells["UserId"]
                    .Value
            );


            // Super Admin cannot deactivate himself
            if (userId == Session.UserId)
            {
                MessageBox.Show(
                    "You cannot change the status of your own Super Admin account!",
                    "Action Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            try
            {
                using var context = new EstateNexusDbContext();

                var user = context.Users.Find(userId);

                if (user == null)
                {
                    MessageBox.Show(
                        "User not found."
                    );

                    return;
                }


                // ======================================
                // DEACTIVATE USER
                // ======================================

                if (user.IsActive)
                {
                    user.IsActive = false;

                    // "Inactive" is NOT allowed by your
                    // database CHECK constraint.
                    // Therefore use "Suspended".
                    user.AccountStatus = "Suspended";

                    context.SaveChanges();

                    MessageBox.Show(
                        "User has been suspended successfully."
                    );
                }


                // ======================================
                // ACTIVATE USER AGAIN
                // ======================================

                else
                {
                    user.IsActive = true;

                    // Valid value according to
                    // CHK_Users_AccountStatus
                    user.AccountStatus = "Pending";

                    context.SaveChanges();

                    MessageBox.Show(
                        "User has been activated successfully.\n\n" +
                        "Account status is now Pending."
                    );
                }


                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating user status:\n\n" +
                    ex.Message,

                    "Error",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // DELETE USER
        // ==========================================

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a user to delete."
                );

                return;
            }


            int userId = Convert.ToInt32(
                dgvUsers.SelectedRows[0]
                    .Cells["UserId"]
                    .Value
            );


            // Prevent Super Admin from deleting himself
            if (userId == Session.UserId)
            {
                MessageBox.Show(
                    "You cannot delete your own Super Admin account!",

                    "Action Denied",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Warning
                );

                return;
            }


            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this user?\n\n" +
                "This action cannot be undone.",

                "Confirm Delete",

                MessageBoxButtons.YesNo,

                MessageBoxIcon.Warning
            );


            if (confirm != DialogResult.Yes)
            {
                return;
            }


            try
            {
                using var context = new EstateNexusDbContext();

                var user = context.Users.Find(userId);

                if (user == null)
                {
                    MessageBox.Show(
                        "User not found."
                    );

                    return;
                }


                context.Users.Remove(user);

                context.SaveChanges();


                MessageBox.Show(
                    "User deleted successfully."
                );


                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting user.\n\n" +
                    ex.Message,

                    "Delete Error",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // DELETE PROPERTY
        // ==========================================

        private void btnDeleteProperty_Click(object sender, EventArgs e)
        {
            if (dgvProperties.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a property to remove."
                );

                return;
            }


            int propertyId = Convert.ToInt32(
                dgvProperties.SelectedRows[0]
                    .Cells["PropertyId"]
                    .Value
            );


            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to remove this property?",

                "Confirm Remove",

                MessageBoxButtons.YesNo,

                MessageBoxIcon.Warning
            );


            if (confirm != DialogResult.Yes)
            {
                return;
            }


            try
            {
                using var context = new EstateNexusDbContext();

                var property = context.Properties.Find(propertyId);

                if (property == null)
                {
                    MessageBox.Show(
                        "Property not found."
                    );

                    return;
                }


                context.Properties.Remove(property);

                context.SaveChanges();


                MessageBox.Show(
                    "Property removed successfully."
                );


                LoadProperties();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error removing property.\n\n" +
                    ex.Message,

                    "Remove Error",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // LOGOUT
        // ==========================================

        private void btnLogout_Click(
            object sender,
            EventArgs e
        )
        {
            Session.Logout();

            LoginForm loginForm =
                new LoginForm();

            loginForm.Show();

            this.Hide();
        }


        // ==========================================
        // CLOSE APPLICATION
        // ==========================================

        protected override void OnFormClosed(
            FormClosedEventArgs e
        )
        {
            base.OnFormClosed(e);

            Environment.Exit(0);
        }
    }
}