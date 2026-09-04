namespace EstateNexus;

partial class AdminDashboard
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
        this.tabMyProperties = new System.Windows.Forms.TabPage();
        this.lblPropertyStats = new System.Windows.Forms.Label();
        this.btnMarkSold = new System.Windows.Forms.Button();
        this.btnDeleteProperty = new System.Windows.Forms.Button();
        this.btnAddProperty = new System.Windows.Forms.Button();
        this.dgvMyProperties = new System.Windows.Forms.DataGridView();
        this.tabVisitRequests = new System.Windows.Forms.TabPage();
        this.btnRejectVisit = new System.Windows.Forms.Button();
        this.btnApproveVisit = new System.Windows.Forms.Button();
        this.btnRefreshVisitRequests = new System.Windows.Forms.Button();
        this.lblVisitFilter = new System.Windows.Forms.Label();
        this.cmbVisitFilter = new System.Windows.Forms.ComboBox();
        this.lblVisitStats = new System.Windows.Forms.Label();
        this.dgvVisitRequests = new System.Windows.Forms.DataGridView();
        this.tabSales = new System.Windows.Forms.TabPage();
        this.lblTotalEarnings = new System.Windows.Forms.Label();
        this.dgvSales = new System.Windows.Forms.DataGridView();

        this.tabControl1.SuspendLayout();
        this.tabMyProperties.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMyProperties)).BeginInit();
        this.tabVisitRequests.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvVisitRequests)).BeginInit();
        this.tabSales.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(15, 12);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(350, 30);
        this.lblTitle.Text = "Seller / Admin Dashboard";

        // btnLogout
        this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.btnLogout.Location = new System.Drawing.Point(860, 12);
        this.btnLogout.Name = "btnLogout";
        this.btnLogout.Size = new System.Drawing.Size(85, 32);
        this.btnLogout.Text = "Logout";
        this.btnLogout.UseVisualStyleBackColor = true;
        this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

        // tabControl1
        this.tabControl1.Controls.Add(this.tabMyProperties);
        this.tabControl1.Controls.Add(this.tabVisitRequests);
        this.tabControl1.Controls.Add(this.tabSales);
        this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.tabControl1.Location = new System.Drawing.Point(15, 55);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(930, 495);
        this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);

        // tabMyProperties
        this.tabMyProperties.Controls.Add(this.lblPropertyStats);
        this.tabMyProperties.Controls.Add(this.btnMarkSold);
        this.tabMyProperties.Controls.Add(this.btnDeleteProperty);
        this.tabMyProperties.Controls.Add(this.btnAddProperty);
        this.tabMyProperties.Controls.Add(this.dgvMyProperties);
        this.tabMyProperties.Location = new System.Drawing.Point(4, 26);
        this.tabMyProperties.Name = "tabMyProperties";
        this.tabMyProperties.Padding = new System.Windows.Forms.Padding(3);
        this.tabMyProperties.Size = new System.Drawing.Size(922, 465);
        this.tabMyProperties.Text = "My Properties (Inventory)";
        this.tabMyProperties.UseVisualStyleBackColor = true;

        // btnAddProperty
        this.btnAddProperty.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnAddProperty.Location = new System.Drawing.Point(8, 8);
        this.btnAddProperty.Name = "btnAddProperty";
        this.btnAddProperty.Size = new System.Drawing.Size(140, 32);
        this.btnAddProperty.Text = "+ Add Property";
        this.btnAddProperty.UseVisualStyleBackColor = true;
        this.btnAddProperty.Click += new System.EventHandler(this.btnAddProperty_Click);

        // btnDeleteProperty
        this.btnDeleteProperty.Location = new System.Drawing.Point(160, 8);
        this.btnDeleteProperty.Name = "btnDeleteProperty";
        this.btnDeleteProperty.Size = new System.Drawing.Size(140, 32);
        this.btnDeleteProperty.Text = "Delete Selected";
        this.btnDeleteProperty.UseVisualStyleBackColor = true;
        this.btnDeleteProperty.Click += new System.EventHandler(this.btnDeleteProperty_Click);

        // btnMarkSold
        this.btnMarkSold.Location = new System.Drawing.Point(310, 8);
        this.btnMarkSold.Name = "btnMarkSold";
        this.btnMarkSold.Size = new System.Drawing.Size(160, 32);
        this.btnMarkSold.Text = "Toggle Sold/Available";
        this.btnMarkSold.UseVisualStyleBackColor = true;
        this.btnMarkSold.Click += new System.EventHandler(this.btnMarkSold_Click);

        // lblPropertyStats
        this.lblPropertyStats.AutoSize = true;
        this.lblPropertyStats.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.lblPropertyStats.Location = new System.Drawing.Point(500, 15);
        this.lblPropertyStats.Name = "lblPropertyStats";
        this.lblPropertyStats.Size = new System.Drawing.Size(250, 19);
        this.lblPropertyStats.Text = "Total: 0 | Available: 0 | Sold: 0";

        // dgvMyProperties
        this.dgvMyProperties.AllowUserToAddRows = false;
        this.dgvMyProperties.AllowUserToDeleteRows = false;
        this.dgvMyProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvMyProperties.Location = new System.Drawing.Point(8, 48);
        this.dgvMyProperties.MultiSelect = false;
        this.dgvMyProperties.Name = "dgvMyProperties";
        this.dgvMyProperties.ReadOnly = true;
        this.dgvMyProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvMyProperties.Size = new System.Drawing.Size(905, 405);

        // tabVisitRequests
        this.tabVisitRequests.Controls.Add(this.lblVisitStats);
        this.tabVisitRequests.Controls.Add(this.cmbVisitFilter);
        this.tabVisitRequests.Controls.Add(this.lblVisitFilter);
        this.tabVisitRequests.Controls.Add(this.btnRefreshVisitRequests);
        this.tabVisitRequests.Controls.Add(this.btnRejectVisit);
        this.tabVisitRequests.Controls.Add(this.btnApproveVisit);
        this.tabVisitRequests.Controls.Add(this.dgvVisitRequests);
        this.tabVisitRequests.Location = new System.Drawing.Point(4, 26);
        this.tabVisitRequests.Name = "tabVisitRequests";
        this.tabVisitRequests.Padding = new System.Windows.Forms.Padding(3);
        this.tabVisitRequests.Size = new System.Drawing.Size(922, 465);
        this.tabVisitRequests.Text = "Visit Requests";
        this.tabVisitRequests.UseVisualStyleBackColor = true;

        // btnApproveVisit
        this.btnApproveVisit.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnApproveVisit.Location = new System.Drawing.Point(8, 8);
        this.btnApproveVisit.Name = "btnApproveVisit";
        this.btnApproveVisit.Size = new System.Drawing.Size(120, 32);
        this.btnApproveVisit.Text = "Approve";
        this.btnApproveVisit.UseVisualStyleBackColor = true;
        this.btnApproveVisit.Click += new System.EventHandler(this.btnApproveVisit_Click);

        // btnRejectVisit
        this.btnRejectVisit.Location = new System.Drawing.Point(135, 8);
        this.btnRejectVisit.Name = "btnRejectVisit";
        this.btnRejectVisit.Size = new System.Drawing.Size(110, 32);
        this.btnRejectVisit.Text = "Reject";
        this.btnRejectVisit.UseVisualStyleBackColor = true;
        this.btnRejectVisit.Click += new System.EventHandler(this.btnRejectVisit_Click);

        // btnRefreshVisitRequests
        this.btnRefreshVisitRequests.Location = new System.Drawing.Point(252, 8);
        this.btnRefreshVisitRequests.Name = "btnRefreshVisitRequests";
        this.btnRefreshVisitRequests.Size = new System.Drawing.Size(100, 32);
        this.btnRefreshVisitRequests.Text = "Refresh";
        this.btnRefreshVisitRequests.UseVisualStyleBackColor = true;
        this.btnRefreshVisitRequests.Click += new System.EventHandler(this.btnRefreshVisitRequests_Click);

        // lblVisitFilter
        this.lblVisitFilter.AutoSize = true;
        this.lblVisitFilter.Location = new System.Drawing.Point(365, 15);
        this.lblVisitFilter.Name = "lblVisitFilter";
        this.lblVisitFilter.Size = new System.Drawing.Size(46, 17);
        this.lblVisitFilter.Text = "Status:";

        // cmbVisitFilter
        this.cmbVisitFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbVisitFilter.FormattingEnabled = true;
        this.cmbVisitFilter.Items.AddRange(new object[] { "All", "Pending", "Approved", "Rejected", "Cancelled" });
        this.cmbVisitFilter.Location = new System.Drawing.Point(415, 11);
        this.cmbVisitFilter.Name = "cmbVisitFilter";
        this.cmbVisitFilter.Size = new System.Drawing.Size(110, 25);
        this.cmbVisitFilter.SelectedIndex = 0;
        this.cmbVisitFilter.SelectedIndexChanged += new System.EventHandler(this.cmbVisitFilter_SelectedIndexChanged);

        // lblVisitStats
        this.lblVisitStats.AutoSize = true;
        this.lblVisitStats.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblVisitStats.ForeColor = System.Drawing.Color.DimGray;
        this.lblVisitStats.Location = new System.Drawing.Point(540, 15);
        this.lblVisitStats.Name = "lblVisitStats";
        this.lblVisitStats.Size = new System.Drawing.Size(60, 17);
        this.lblVisitStats.Text = "Total: 0";

        // dgvVisitRequests
        this.dgvVisitRequests.AllowUserToAddRows = false;
        this.dgvVisitRequests.AllowUserToDeleteRows = false;
        this.dgvVisitRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvVisitRequests.Location = new System.Drawing.Point(8, 48);
        this.dgvVisitRequests.MultiSelect = false;
        this.dgvVisitRequests.Name = "dgvVisitRequests";
        this.dgvVisitRequests.ReadOnly = true;
        this.dgvVisitRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvVisitRequests.Size = new System.Drawing.Size(905, 405);

        // tabSales
        this.tabSales.Controls.Add(this.lblTotalEarnings);
        this.tabSales.Controls.Add(this.dgvSales);
        this.tabSales.Location = new System.Drawing.Point(4, 26);
        this.tabSales.Name = "tabSales";
        this.tabSales.Padding = new System.Windows.Forms.Padding(3);
        this.tabSales.Size = new System.Drawing.Size(922, 465);
        this.tabSales.Text = "Sales & Earnings";
        this.tabSales.UseVisualStyleBackColor = true;

        // lblTotalEarnings
        this.lblTotalEarnings.AutoSize = true;
        this.lblTotalEarnings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.lblTotalEarnings.ForeColor = System.Drawing.Color.DarkGreen;
        this.lblTotalEarnings.Location = new System.Drawing.Point(10, 12);
        this.lblTotalEarnings.Name = "lblTotalEarnings";
        this.lblTotalEarnings.Size = new System.Drawing.Size(300, 21);
        this.lblTotalEarnings.Text = "Total Earnings from Sales: ৳0.00";

        // dgvSales
        this.dgvSales.AllowUserToAddRows = false;
        this.dgvSales.AllowUserToDeleteRows = false;
        this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvSales.Location = new System.Drawing.Point(8, 45);
        this.dgvSales.Name = "dgvSales";
        this.dgvSales.ReadOnly = true;
        this.dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvSales.Size = new System.Drawing.Size(905, 405);

        // AdminDashboard
        this.ClientSize = new System.Drawing.Size(960, 565);
        this.Controls.Add(this.tabControl1);
        this.Controls.Add(this.btnLogout);
        this.Controls.Add(this.lblTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "AdminDashboard";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "EstateNexus - Seller Dashboard";
        this.Load += new System.EventHandler(this.AdminDashboard_Load);
        this.tabControl1.ResumeLayout(false);
        this.tabMyProperties.ResumeLayout(false);
        this.tabMyProperties.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMyProperties)).EndInit();
        this.tabVisitRequests.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvVisitRequests)).EndInit();
        this.tabSales.ResumeLayout(false);
        this.tabSales.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabMyProperties;
    private System.Windows.Forms.DataGridView dgvMyProperties;
    private System.Windows.Forms.Button btnAddProperty;
    private System.Windows.Forms.Button btnDeleteProperty;
    private System.Windows.Forms.Button btnMarkSold;
    private System.Windows.Forms.Label lblPropertyStats;
    private System.Windows.Forms.TabPage tabVisitRequests;
    private System.Windows.Forms.DataGridView dgvVisitRequests;
    private System.Windows.Forms.Button btnApproveVisit;
    private System.Windows.Forms.Button btnRejectVisit;
    private System.Windows.Forms.Button btnRefreshVisitRequests;
    private System.Windows.Forms.Label lblVisitFilter;
    private System.Windows.Forms.ComboBox cmbVisitFilter;
    private System.Windows.Forms.Label lblVisitStats;
    private System.Windows.Forms.TabPage tabSales;
    private System.Windows.Forms.DataGridView dgvSales;
    private System.Windows.Forms.Label lblTotalEarnings;
}
