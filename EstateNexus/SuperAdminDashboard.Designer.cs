namespace EstateNexus;

partial class SuperAdminDashboard
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblTitle = new System.Windows.Forms.Label();
        this.btnLogout = new System.Windows.Forms.Button();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabUsers = new System.Windows.Forms.TabPage();
        this.btnToggleStatus = new System.Windows.Forms.Button();
        this.btnApproveUser = new System.Windows.Forms.Button();
        this.btnRejectUser = new System.Windows.Forms.Button();
        this.btnDeleteUser = new System.Windows.Forms.Button();
        this.cmbUserStatusFilter = new System.Windows.Forms.ComboBox();
        this.cmbUserRoleFilter = new System.Windows.Forms.ComboBox();
        this.lblPendingCount = new System.Windows.Forms.Label();
        this.dgvUsers = new System.Windows.Forms.DataGridView();
        this.tabProperties = new System.Windows.Forms.TabPage();
        this.btnDeleteProperty = new System.Windows.Forms.Button();
        this.dgvProperties = new System.Windows.Forms.DataGridView();
        this.tabRevenue = new System.Windows.Forms.TabPage();
        this.lblCommission = new System.Windows.Forms.Label();
        this.lblRevenue = new System.Windows.Forms.Label();
        this.dgvAllOrders = new System.Windows.Forms.DataGridView();

        this.tabControl1.SuspendLayout();
        this.tabUsers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
        this.tabProperties.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).BeginInit();
        this.tabRevenue.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvAllOrders)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(15, 12);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(350, 30);
        this.lblTitle.Text = "Super Admin Dashboard";

        // btnLogout
        this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.btnLogout.Location = new System.Drawing.Point(860, 12);
        this.btnLogout.Name = "btnLogout";
        this.btnLogout.Size = new System.Drawing.Size(85, 32);
        this.btnLogout.Text = "Logout";
        this.btnLogout.UseVisualStyleBackColor = true;
        this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

        // tabControl1
        this.tabControl1.Controls.Add(this.tabUsers);
        this.tabControl1.Controls.Add(this.tabProperties);
        this.tabControl1.Controls.Add(this.tabRevenue);
        this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.tabControl1.Location = new System.Drawing.Point(15, 55);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(930, 495);

        // tabUsers
        this.tabUsers.Controls.Add(this.btnToggleStatus);
        this.tabUsers.Controls.Add(this.btnApproveUser);
        this.tabUsers.Controls.Add(this.btnRejectUser);
        this.tabUsers.Controls.Add(this.btnDeleteUser);
        this.tabUsers.Controls.Add(this.cmbUserStatusFilter);
        this.tabUsers.Controls.Add(this.cmbUserRoleFilter);
        this.tabUsers.Controls.Add(this.lblPendingCount);
        this.tabUsers.Controls.Add(this.dgvUsers);
        this.tabUsers.Location = new System.Drawing.Point(4, 26);
        this.tabUsers.Name = "tabUsers";
        this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
        this.tabUsers.Size = new System.Drawing.Size(922, 465);
        this.tabUsers.Text = "User Management";
        this.tabUsers.UseVisualStyleBackColor = true;

        // btnToggleStatus
        this.btnToggleStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnToggleStatus.Location = new System.Drawing.Point(8, 8);
        this.btnToggleStatus.Name = "btnToggleStatus";
        this.btnToggleStatus.Size = new System.Drawing.Size(100, 32);
        this.btnToggleStatus.Text = "Suspend";
        this.btnToggleStatus.UseVisualStyleBackColor = true;
        this.btnToggleStatus.Click += new System.EventHandler(this.btnToggleStatus_Click);

        // btnApproveUser
        this.btnApproveUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnApproveUser.Location = new System.Drawing.Point(114, 8);
        this.btnApproveUser.Name = "btnApproveUser";
        this.btnApproveUser.Size = new System.Drawing.Size(80, 32);
        this.btnApproveUser.Text = "Approve";
        this.btnApproveUser.UseVisualStyleBackColor = true;
        this.btnApproveUser.Click += new System.EventHandler(this.btnApproveUser_Click);

        // btnRejectUser
        this.btnRejectUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnRejectUser.Location = new System.Drawing.Point(200, 8);
        this.btnRejectUser.Name = "btnRejectUser";
        this.btnRejectUser.Size = new System.Drawing.Size(80, 32);
        this.btnRejectUser.Text = "Reject";
        this.btnRejectUser.UseVisualStyleBackColor = true;
        this.btnRejectUser.Click += new System.EventHandler(this.btnRejectUser_Click);

        // btnDeleteUser
        this.btnDeleteUser.Location = new System.Drawing.Point(286, 8);
        this.btnDeleteUser.Name = "btnDeleteUser";
        this.btnDeleteUser.Size = new System.Drawing.Size(95, 32);
        this.btnDeleteUser.Text = "Delete User";
        this.btnDeleteUser.UseVisualStyleBackColor = true;
        this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);

        // cmbUserStatusFilter
        this.cmbUserStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbUserStatusFilter.FormattingEnabled = true;
        this.cmbUserStatusFilter.Location = new System.Drawing.Point(390, 11);
        this.cmbUserStatusFilter.Name = "cmbUserStatusFilter";
        this.cmbUserStatusFilter.Size = new System.Drawing.Size(105, 25);
        this.cmbUserStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cmbUserStatusFilter_SelectedIndexChanged);

        // cmbUserRoleFilter
        this.cmbUserRoleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbUserRoleFilter.FormattingEnabled = true;
        this.cmbUserRoleFilter.Location = new System.Drawing.Point(502, 11);
        this.cmbUserRoleFilter.Name = "cmbUserRoleFilter";
        this.cmbUserRoleFilter.Size = new System.Drawing.Size(105, 25);
        this.cmbUserRoleFilter.SelectedIndexChanged += new System.EventHandler(this.cmbUserRoleFilter_SelectedIndexChanged);

        // lblPendingCount
        this.lblPendingCount.AutoSize = true;
        this.lblPendingCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblPendingCount.ForeColor = System.Drawing.Color.DarkRed;
        this.lblPendingCount.Location = new System.Drawing.Point(615, 15);
        this.lblPendingCount.Name = "lblPendingCount";
        this.lblPendingCount.Size = new System.Drawing.Size(140, 17);
        this.lblPendingCount.Text = "Pending approvals: 0";

        // dgvUsers
        this.dgvUsers.AllowUserToAddRows = false;
        this.dgvUsers.AllowUserToDeleteRows = false;
        this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvUsers.Location = new System.Drawing.Point(8, 48);
        this.dgvUsers.MultiSelect = false;
        this.dgvUsers.Name = "dgvUsers";
        this.dgvUsers.ReadOnly = true;
        this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvUsers.Size = new System.Drawing.Size(905, 405);
        this.dgvUsers.SelectionChanged += new System.EventHandler(this.dgvUsers_SelectionChanged);

        // tabProperties
        this.tabProperties.Controls.Add(this.btnDeleteProperty);
        this.tabProperties.Controls.Add(this.dgvProperties);
        this.tabProperties.Location = new System.Drawing.Point(4, 26);
        this.tabProperties.Name = "tabProperties";
        this.tabProperties.Padding = new System.Windows.Forms.Padding(3);
        this.tabProperties.Size = new System.Drawing.Size(922, 465);
        this.tabProperties.Text = "Platform Properties";
        this.tabProperties.UseVisualStyleBackColor = true;

        // btnDeleteProperty
        this.btnDeleteProperty.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnDeleteProperty.Location = new System.Drawing.Point(8, 8);
        this.btnDeleteProperty.Name = "btnDeleteProperty";
        this.btnDeleteProperty.Size = new System.Drawing.Size(160, 32);
        this.btnDeleteProperty.Text = "Remove Property";
        this.btnDeleteProperty.UseVisualStyleBackColor = true;
        this.btnDeleteProperty.Click += new System.EventHandler(this.btnDeleteProperty_Click);

        // dgvProperties
        this.dgvProperties.AllowUserToAddRows = false;
        this.dgvProperties.AllowUserToDeleteRows = false;
        this.dgvProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvProperties.Location = new System.Drawing.Point(8, 48);
        this.dgvProperties.MultiSelect = false;
        this.dgvProperties.Name = "dgvProperties";
        this.dgvProperties.ReadOnly = true;
        this.dgvProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvProperties.Size = new System.Drawing.Size(905, 405);

        // tabRevenue
        this.tabRevenue.Controls.Add(this.dgvAllOrders);
        this.tabRevenue.Controls.Add(this.lblCommission);
        this.tabRevenue.Controls.Add(this.lblRevenue);
        this.tabRevenue.Location = new System.Drawing.Point(4, 26);
        this.tabRevenue.Name = "tabRevenue";
        this.tabRevenue.Padding = new System.Windows.Forms.Padding(3);
        this.tabRevenue.Size = new System.Drawing.Size(922, 465);
        this.tabRevenue.Text = "Revenue & Orders";
        this.tabRevenue.UseVisualStyleBackColor = true;

        // lblRevenue
        this.lblRevenue.AutoSize = true;
        this.lblRevenue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        this.lblRevenue.Location = new System.Drawing.Point(10, 12);
        this.lblRevenue.Name = "lblRevenue";
        this.lblRevenue.Size = new System.Drawing.Size(260, 20);
        this.lblRevenue.Text = "Total Marketplace Volume: ৳0.00";

        // lblCommission
        this.lblCommission.AutoSize = true;
        this.lblCommission.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        this.lblCommission.ForeColor = System.Drawing.Color.DarkGreen;
        this.lblCommission.Location = new System.Drawing.Point(400, 12);
        this.lblCommission.Name = "lblCommission";
        this.lblCommission.Size = new System.Drawing.Size(260, 20);
        this.lblCommission.Text = "Platform Commission (5%): ৳0.00";

        // dgvAllOrders
        this.dgvAllOrders.AllowUserToAddRows = false;
        this.dgvAllOrders.AllowUserToDeleteRows = false;
        this.dgvAllOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvAllOrders.Location = new System.Drawing.Point(8, 48);
        this.dgvAllOrders.Name = "dgvAllOrders";
        this.dgvAllOrders.ReadOnly = true;
        this.dgvAllOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvAllOrders.Size = new System.Drawing.Size(905, 405);

        // SuperAdminDashboard
        this.ClientSize = new System.Drawing.Size(960, 565);
        this.Controls.Add(this.tabControl1);
        this.Controls.Add(this.btnLogout);
        this.Controls.Add(this.lblTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "SuperAdminDashboard";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "EstateNexus - Super Admin";
        this.Load += new System.EventHandler(this.SuperAdminDashboard_Load);
        this.tabControl1.ResumeLayout(false);
        this.tabUsers.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
        this.tabProperties.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).EndInit();
        this.tabRevenue.ResumeLayout(false);
        this.tabRevenue.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvAllOrders)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabUsers;
    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.Button btnToggleStatus;
    private System.Windows.Forms.Button btnApproveUser;
    private System.Windows.Forms.Button btnRejectUser;
    private System.Windows.Forms.Button btnDeleteUser;
    private System.Windows.Forms.ComboBox cmbUserStatusFilter;
    private System.Windows.Forms.ComboBox cmbUserRoleFilter;
    private System.Windows.Forms.Label lblPendingCount;
    private System.Windows.Forms.TabPage tabProperties;
    private System.Windows.Forms.DataGridView dgvProperties;
    private System.Windows.Forms.Button btnDeleteProperty;
    private System.Windows.Forms.TabPage tabRevenue;
    private System.Windows.Forms.Label lblRevenue;
    private System.Windows.Forms.Label lblCommission;
    private System.Windows.Forms.DataGridView dgvAllOrders;
}
