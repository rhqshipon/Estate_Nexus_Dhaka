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

            // Populate status filter dropdown if empty
            if (cmbUserStatusFilter.Items.Count == 0)
            {
                cmbUserStatusFilter.Items.AddRange(new object[] { "All", "Pending", "Active", "Suspended" });
                cmbUserStatusFilter.SelectedIndex = 0;
            }

            // Populate role filter dropdown if empty
            if (cmbUserRoleFilter.Items.Count == 0)
            {
                cmbUserRoleFilter.Items.AddRange(new object[] { "All", "Customer", "Admin", "SuperAdmin" });
                cmbUserRoleFilter.SelectedIndex = 0;
            }

            UpdatePendingCount();
            LoadUsers();
            LoadProperties();
            LoadRevenue();
        }


        // ==========================================
        // UPDATE PENDING APPROVALS COUNT
        // ==========================================

        private void UpdatePendingCount()
        {
            try
            {
                using var context = new EstateNexusDbContext();

                // Count all users with AccountStatus = "Pending" regardless of role
                int count = context.Users.Count(u => u.AccountStatus == "Pending");

                lblPendingCount.Text = "Pending approvals: " + count;
            }
            catch
            {
                lblPendingCount.Text = "Pending approvals: 0";
            }
        }


        // ==========================================
        // LOAD USERS (WITH FILTERS & ROLE FROM DB)
        // ==========================================

        private void LoadUsers()
        {
            try
            {
                using var context = new EstateNexusDbContext();

                // Query users including Role from database relationship
                var userList = context.Users
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

                // ------------------------------------------
                // Filter by Account Status (normalizing legacy values for display only)
                // ------------------------------------------
                string selectedStatus = cmbUserStatusFilter.SelectedItem != null
                    ? cmbUserStatusFilter.SelectedItem.ToString()
                    : "All";

                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All")
                {
                    userList = userList.Where(u =>
                    {
                        string norm = (u.AccountStatus ?? "").Trim();

                        // Normalize legacy statuses for filter view only
                        if (string.Equals(norm, "Approved", StringComparison.OrdinalIgnoreCase))
                        {
                            norm = "Active";
                        }
                        else if (string.Equals(norm, "Rejected", StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(norm, "Inactive", StringComparison.OrdinalIgnoreCase))
                        {
                            norm = "Suspended";
                        }

                        return string.Equals(norm, selectedStatus, StringComparison.OrdinalIgnoreCase);
                    }).ToList();
                }

                // ------------------------------------------
                // Filter by Role
                // ------------------------------------------
                string selectedRole = cmbUserRoleFilter.SelectedItem != null
                    ? cmbUserRoleFilter.SelectedItem.ToString()
                    : "All";

                if (!string.IsNullOrEmpty(selectedRole) && selectedRole != "All")
                {
                    userList = userList.Where(u =>
                        string.Equals(u.Role, selectedRole, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Bind to grid
                dgvUsers.DataSource = userList;

                // Update pending count label and button enabled states
                UpdatePendingCount();
                UpdateUserButtonStates();
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
        // FILTER EVENT HANDLERS
        // ==========================================

        private void cmbUserStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void cmbUserRoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }


        // ==========================================
        // GRID SELECTION & BUTTON STATE MANAGEMENT
        // ==========================================

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateUserButtonStates();
        }

        private void UpdateUserButtonStates()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                btnToggleStatus.Enabled = false;
                btnApproveUser.Enabled = false;
                btnRejectUser.Enabled = false;
                btnDeleteUser.Enabled = false;
                btnToggleStatus.Text = "Suspend";
                return;
            }

            var row = dgvUsers.SelectedRows[0];
            int userId = Convert.ToInt32(row.Cells["UserId"].Value);
            string role = row.Cells["Role"].Value?.ToString() ?? "";
            string accountStatus = row.Cells["AccountStatus"].Value?.ToString() ?? "";

            bool isSelf = (userId == Session.UserId);
            bool isSuperAdmin = string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase);
            bool isPending = string.Equals(accountStatus, "Pending", StringComparison.OrdinalIgnoreCase);

            // Normalize status to determine toggle action
            string normStatus = (accountStatus ?? "").Trim();
            if (string.Equals(normStatus, "Approved", StringComparison.OrdinalIgnoreCase))
            {
                normStatus = "Active";
            }
            else if (string.Equals(normStatus, "Rejected", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(normStatus, "Inactive", StringComparison.OrdinalIgnoreCase))
            {
                normStatus = "Suspended";
            }

            // Set Toggle button text based on normalized status
            if (string.Equals(normStatus, "Active", StringComparison.OrdinalIgnoreCase))
            {
                btnToggleStatus.Text = "Suspend";
            }
            else if (string.Equals(normStatus, "Suspended", StringComparison.OrdinalIgnoreCase))
            {
                btnToggleStatus.Text = "Activate";
            }
            else
            {
                btnToggleStatus.Text = "Toggle Status";
            }

            // Toggle is enabled only for non-self, non-SuperAdmin, non-Pending users
            btnToggleStatus.Enabled = !isSelf && !isSuperAdmin && !isPending;

            // Approve & Reject are enabled ONLY for Pending users (never for self or SuperAdmin)
            btnApproveUser.Enabled = !isSelf && !isSuperAdmin && isPending;
            btnRejectUser.Enabled = !isSelf && !isSuperAdmin && isPending;

            // Delete is disabled for self and SuperAdmin
            btnDeleteUser.Enabled = !isSelf && !isSuperAdmin;
        }


        // ==========================================
        // APPROVE PENDING USER
        // ==========================================

        private void btnApproveUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

            // Self-protection
            if (userId == Session.UserId)
            {
                MessageBox.Show(
                    "You cannot approve your own Super Admin account!",
                    "Action Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                using var context = new EstateNexusDbContext();

                var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                if (user.Role != null && string.Equals(user.Role.RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Super Admin accounts cannot be approved.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Approve user: AccountStatus = "Active", IsActive = true
                user.AccountStatus = "Active";
                user.IsActive = true;

                context.SaveChanges();

                MessageBox.Show(
                    "User approved successfully!",
                    "Approved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error approving user:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // REJECT PENDING USER
        // ==========================================

        private void btnRejectUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

            // Self-protection
            if (userId == Session.UserId)
            {
                MessageBox.Show(
                    "You cannot reject your own Super Admin account!",
                    "Action Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Confirmation prompt
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to reject this user?",
                "Confirm Reject",
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

                var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                if (user.Role != null && string.Equals(user.Role.RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Super Admin accounts cannot be rejected.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Reject user: AccountStatus = "Suspended", IsActive = false
                user.AccountStatus = "Suspended";
                user.IsActive = false;

                context.SaveChanges();

                MessageBox.Show(
                    "User rejected and suspended successfully.",
                    "Rejected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error rejecting user:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ==========================================
        // TOGGLE USER ACTIVE / SUSPENDED
        // ==========================================

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

            // Super Admin cannot change his own account
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

                var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                if (user.Role != null && string.Equals(user.Role.RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "You cannot change the status of a Super Admin account!",
                        "Action Denied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                string status = (user.AccountStatus ?? "").Trim();
                if (string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This user account is Pending. Please use Approve or Reject instead.",
                        "Action Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                // Toggle direction based on normalized status
                bool isCurrentlyActive = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase);

                if (isCurrentlyActive)
                {
                    // SUSPEND
                    user.AccountStatus = "Suspended";
                    user.IsActive = false;

                    context.SaveChanges();

                    MessageBox.Show("User has been suspended successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // ACTIVATE
                    user.AccountStatus = "Active";
                    user.IsActive = true;

                    context.SaveChanges();

                    MessageBox.Show("User has been activated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating user status:\n\n" + ex.Message,
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