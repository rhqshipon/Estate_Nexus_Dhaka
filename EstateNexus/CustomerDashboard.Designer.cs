namespace EstateNexus;

partial class CustomerDashboard
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
        this.tabBrowse = new System.Windows.Forms.TabPage();
        this.lblFilter = new System.Windows.Forms.Label();
        this.cmbListingTypeFilter = new System.Windows.Forms.ComboBox();
        this.txtSearch = new System.Windows.Forms.TextBox();
        this.btnSearch = new System.Windows.Forms.Button();
        this.dgvBrowseProperties = new System.Windows.Forms.DataGridView();
        this.btnAddToCart = new System.Windows.Forms.Button();
        this.btnRequestVisit = new System.Windows.Forms.Button();
        this.tabCart = new System.Windows.Forms.TabPage();
        this.lblCartTotal = new System.Windows.Forms.Label();
        this.btnRemoveFromCart = new System.Windows.Forms.Button();
        this.dgvCart = new System.Windows.Forms.DataGridView();
        this.btnCheckout = new System.Windows.Forms.Button();
        this.tabOrders = new System.Windows.Forms.TabPage();
        this.dgvOrders = new System.Windows.Forms.DataGridView();
        this.tabMyVisits = new System.Windows.Forms.TabPage();
        this.dgvMyVisits = new System.Windows.Forms.DataGridView();
        this.tabReviews = new System.Windows.Forms.TabPage();
        this.lblSelectProperty = new System.Windows.Forms.Label();
        this.cmbReviewProperty = new System.Windows.Forms.ComboBox();
        this.lblRating = new System.Windows.Forms.Label();
        this.numRating = new System.Windows.Forms.NumericUpDown();
        this.lblComment = new System.Windows.Forms.Label();
        this.txtReviewComment = new System.Windows.Forms.TextBox();
        this.btnSubmitReview = new System.Windows.Forms.Button();
        this.dgvReviews = new System.Windows.Forms.DataGridView();

        this.tabControl1.SuspendLayout();
        this.tabBrowse.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvBrowseProperties)).BeginInit();
        this.tabCart.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
        this.tabOrders.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
        this.tabMyVisits.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMyVisits)).BeginInit();
        this.tabReviews.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numRating)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvReviews)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(15, 12);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(350, 30);
        this.lblTitle.Text = "Customer Dashboard";

        // btnLogout
        this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.btnLogout.Location = new System.Drawing.Point(860, 12);
        this.btnLogout.Name = "btnLogout";
        this.btnLogout.Size = new System.Drawing.Size(85, 32);
        this.btnLogout.Text = "Logout";
        this.btnLogout.UseVisualStyleBackColor = true;
        this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

        // tabControl1
        this.tabControl1.Controls.Add(this.tabBrowse);
        this.tabControl1.Controls.Add(this.tabCart);
        this.tabControl1.Controls.Add(this.tabOrders);
        this.tabControl1.Controls.Add(this.tabMyVisits);
        this.tabControl1.Controls.Add(this.tabReviews);
        this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.tabControl1.Location = new System.Drawing.Point(15, 55);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(930, 495);

        // tabBrowse
        this.tabBrowse.Controls.Add(this.lblFilter);
        this.tabBrowse.Controls.Add(this.cmbListingTypeFilter);
        this.tabBrowse.Controls.Add(this.txtSearch);
        this.tabBrowse.Controls.Add(this.btnSearch);
        this.tabBrowse.Controls.Add(this.dgvBrowseProperties);
        this.tabBrowse.Controls.Add(this.btnAddToCart);
        this.tabBrowse.Controls.Add(this.btnRequestVisit);
        this.tabBrowse.Location = new System.Drawing.Point(4, 26);
        this.tabBrowse.Name = "tabBrowse";
        this.tabBrowse.Padding = new System.Windows.Forms.Padding(3);
        this.tabBrowse.Size = new System.Drawing.Size(922, 465);
        this.tabBrowse.Text = "Browse Properties";
        this.tabBrowse.UseVisualStyleBackColor = true;

        // txtSearch
        this.txtSearch.Location = new System.Drawing.Point(8, 10);
        this.txtSearch.Name = "txtSearch";
        this.txtSearch.PlaceholderText = "Search by property title or location...";
        this.txtSearch.Size = new System.Drawing.Size(300, 24);

        // lblFilter
        this.lblFilter.AutoSize = true;
        this.lblFilter.Location = new System.Drawing.Point(320, 13);
        this.lblFilter.Name = "lblFilter";
        this.lblFilter.Size = new System.Drawing.Size(38, 17);
        this.lblFilter.Text = "Type:";

        // cmbListingTypeFilter
        this.cmbListingTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbListingTypeFilter.FormattingEnabled = true;
        this.cmbListingTypeFilter.Items.AddRange(new object[] { "All", "Sale", "Rent" });
        this.cmbListingTypeFilter.Location = new System.Drawing.Point(365, 10);
        this.cmbListingTypeFilter.Name = "cmbListingTypeFilter";
        this.cmbListingTypeFilter.Size = new System.Drawing.Size(100, 25);

        // btnSearch
        this.btnSearch.Location = new System.Drawing.Point(480, 9);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(95, 27);
        this.btnSearch.Text = "Search";
        this.btnSearch.UseVisualStyleBackColor = true;
        this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

        // dgvBrowseProperties
        this.dgvBrowseProperties.AllowUserToAddRows = false;
        this.dgvBrowseProperties.AllowUserToDeleteRows = false;
        this.dgvBrowseProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvBrowseProperties.Location = new System.Drawing.Point(8, 45);
        this.dgvBrowseProperties.MultiSelect = false;
        this.dgvBrowseProperties.Name = "dgvBrowseProperties";
        this.dgvBrowseProperties.ReadOnly = true;
        this.dgvBrowseProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvBrowseProperties.Size = new System.Drawing.Size(905, 365);

        // btnAddToCart
        this.btnAddToCart.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnAddToCart.Location = new System.Drawing.Point(8, 420);
        this.btnAddToCart.Name = "btnAddToCart";
        this.btnAddToCart.Size = new System.Drawing.Size(160, 35);
        this.btnAddToCart.Text = "+ Add to Cart";
        this.btnAddToCart.UseVisualStyleBackColor = true;
        this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);

        // btnRequestVisit
        this.btnRequestVisit.Location = new System.Drawing.Point(180, 420);
        this.btnRequestVisit.Name = "btnRequestVisit";
        this.btnRequestVisit.Size = new System.Drawing.Size(160, 35);
        this.btnRequestVisit.Text = "Schedule Visit";
        this.btnRequestVisit.UseVisualStyleBackColor = true;
        this.btnRequestVisit.Click += new System.EventHandler(this.btnRequestVisit_Click);

        // tabCart
        this.tabCart.Controls.Add(this.lblCartTotal);
        this.tabCart.Controls.Add(this.btnRemoveFromCart);
        this.tabCart.Controls.Add(this.dgvCart);
        this.tabCart.Controls.Add(this.btnCheckout);
        this.tabCart.Location = new System.Drawing.Point(4, 26);
        this.tabCart.Name = "tabCart";
        this.tabCart.Padding = new System.Windows.Forms.Padding(3);
        this.tabCart.Size = new System.Drawing.Size(922, 465);
        this.tabCart.Text = "My Cart";
        this.tabCart.UseVisualStyleBackColor = true;

        // dgvCart
        this.dgvCart.AllowUserToAddRows = false;
        this.dgvCart.AllowUserToDeleteRows = false;
        this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvCart.Location = new System.Drawing.Point(8, 8);
        this.dgvCart.MultiSelect = false;
        this.dgvCart.Name = "dgvCart";
        this.dgvCart.ReadOnly = true;
        this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvCart.Size = new System.Drawing.Size(905, 400);

        // btnRemoveFromCart
        this.btnRemoveFromCart.Location = new System.Drawing.Point(8, 420);
        this.btnRemoveFromCart.Name = "btnRemoveFromCart";
        this.btnRemoveFromCart.Size = new System.Drawing.Size(150, 35);
        this.btnRemoveFromCart.Text = "Remove Item";
        this.btnRemoveFromCart.UseVisualStyleBackColor = true;
        this.btnRemoveFromCart.Click += new System.EventHandler(this.btnRemoveFromCart_Click);

        // btnCheckout
        this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnCheckout.Location = new System.Drawing.Point(170, 420);
        this.btnCheckout.Name = "btnCheckout";
        this.btnCheckout.Size = new System.Drawing.Size(190, 35);
        this.btnCheckout.Text = "Proceed to Checkout";
        this.btnCheckout.UseVisualStyleBackColor = true;
        this.btnCheckout.Click += new System.EventHandler(this.btnCheckout_Click);

        // lblCartTotal
        this.lblCartTotal.AutoSize = true;
        this.lblCartTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.lblCartTotal.ForeColor = System.Drawing.Color.DarkGreen;
        this.lblCartTotal.Location = new System.Drawing.Point(450, 426);
        this.lblCartTotal.Name = "lblCartTotal";
        this.lblCartTotal.Size = new System.Drawing.Size(171, 21);
        this.lblCartTotal.Text = "Total Amount: ৳0.00";

        // tabOrders
        this.tabOrders.Controls.Add(this.dgvOrders);
        this.tabOrders.Location = new System.Drawing.Point(4, 26);
        this.tabOrders.Name = "tabOrders";
        this.tabOrders.Padding = new System.Windows.Forms.Padding(3);
        this.tabOrders.Size = new System.Drawing.Size(922, 465);
        this.tabOrders.Text = "My Orders & History";
        this.tabOrders.UseVisualStyleBackColor = true;

        // dgvOrders
        this.dgvOrders.AllowUserToAddRows = false;
        this.dgvOrders.AllowUserToDeleteRows = false;
        this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvOrders.Location = new System.Drawing.Point(3, 3);
        this.dgvOrders.Name = "dgvOrders";
        this.dgvOrders.ReadOnly = true;
        this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvOrders.Size = new System.Drawing.Size(916, 459);

        // tabMyVisits
        this.tabMyVisits.Controls.Add(this.dgvMyVisits);
        this.tabMyVisits.Location = new System.Drawing.Point(4, 26);
        this.tabMyVisits.Name = "tabMyVisits";
        this.tabMyVisits.Padding = new System.Windows.Forms.Padding(3);
        this.tabMyVisits.Size = new System.Drawing.Size(922, 465);
        this.tabMyVisits.Text = "My Visit Requests";
        this.tabMyVisits.UseVisualStyleBackColor = true;

        // dgvMyVisits
        this.dgvMyVisits.AllowUserToAddRows = false;
        this.dgvMyVisits.AllowUserToDeleteRows = false;
        this.dgvMyVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvMyVisits.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvMyVisits.Location = new System.Drawing.Point(3, 3);
        this.dgvMyVisits.Name = "dgvMyVisits";
        this.dgvMyVisits.ReadOnly = true;
        this.dgvMyVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvMyVisits.Size = new System.Drawing.Size(916, 459);

        // tabReviews
        this.tabReviews.Controls.Add(this.lblSelectProperty);
        this.tabReviews.Controls.Add(this.cmbReviewProperty);
        this.tabReviews.Controls.Add(this.lblRating);
        this.tabReviews.Controls.Add(this.numRating);
        this.tabReviews.Controls.Add(this.lblComment);
        this.tabReviews.Controls.Add(this.txtReviewComment);
        this.tabReviews.Controls.Add(this.btnSubmitReview);
        this.tabReviews.Controls.Add(this.dgvReviews);
        this.tabReviews.Location = new System.Drawing.Point(4, 26);
        this.tabReviews.Name = "tabReviews";
        this.tabReviews.Padding = new System.Windows.Forms.Padding(3);
        this.tabReviews.Size = new System.Drawing.Size(922, 465);
        this.tabReviews.Text = "Reviews & Ratings";
        this.tabReviews.UseVisualStyleBackColor = true;

        // dgvReviews
        this.dgvReviews.AllowUserToAddRows = false;
        this.dgvReviews.AllowUserToDeleteRows = false;
        this.dgvReviews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvReviews.Location = new System.Drawing.Point(8, 8);
        this.dgvReviews.MultiSelect = false;
        this.dgvReviews.Name = "dgvReviews";
        this.dgvReviews.ReadOnly = true;
        this.dgvReviews.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvReviews.Size = new System.Drawing.Size(905, 360);

        // lblSelectProperty
        this.lblSelectProperty.AutoSize = true;
        this.lblSelectProperty.Location = new System.Drawing.Point(8, 385);
        this.lblSelectProperty.Name = "lblSelectProperty";
        this.lblSelectProperty.Size = new System.Drawing.Size(99, 17);
        this.lblSelectProperty.Text = "Select Property:";

        // cmbReviewProperty
        this.cmbReviewProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbReviewProperty.FormattingEnabled = true;
        this.cmbReviewProperty.Location = new System.Drawing.Point(115, 382);
        this.cmbReviewProperty.Name = "cmbReviewProperty";
        this.cmbReviewProperty.Size = new System.Drawing.Size(400, 25);

        // lblRating
        this.lblRating.AutoSize = true;
        this.lblRating.Location = new System.Drawing.Point(540, 385);
        this.lblRating.Name = "lblRating";
        this.lblRating.Size = new System.Drawing.Size(81, 17);
        this.lblRating.Text = "Rating (1-5):";

        // numRating
        this.numRating.Location = new System.Drawing.Point(625, 382);
        this.numRating.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
        this.numRating.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numRating.Name = "numRating";
        this.numRating.Size = new System.Drawing.Size(50, 24);
        this.numRating.Value = new decimal(new int[] { 5, 0, 0, 0 });

        // lblComment
        this.lblComment.AutoSize = true;
        this.lblComment.Location = new System.Drawing.Point(8, 425);
        this.lblComment.Name = "lblComment";
        this.lblComment.Size = new System.Drawing.Size(67, 17);
        this.lblComment.Text = "Comment:";

        // txtReviewComment
        this.txtReviewComment.Location = new System.Drawing.Point(115, 422);
        this.txtReviewComment.Name = "txtReviewComment";
        this.txtReviewComment.PlaceholderText = "Write your review feedback here...";
        this.txtReviewComment.Size = new System.Drawing.Size(560, 24);

        // btnSubmitReview
        this.btnSubmitReview.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnSubmitReview.Location = new System.Drawing.Point(690, 418);
        this.btnSubmitReview.Name = "btnSubmitReview";
        this.btnSubmitReview.Size = new System.Drawing.Size(150, 32);
        this.btnSubmitReview.Text = "Submit Review";
        this.btnSubmitReview.UseVisualStyleBackColor = true;
        this.btnSubmitReview.Click += new System.EventHandler(this.btnSubmitReview_Click);

        // CustomerDashboard
        this.ClientSize = new System.Drawing.Size(960, 565);
        this.Controls.Add(this.tabControl1);
        this.Controls.Add(this.btnLogout);
        this.Controls.Add(this.lblTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "CustomerDashboard";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "EstateNexus - Customer Dashboard";
        this.Load += new System.EventHandler(this.CustomerDashboard_Load);
        this.tabControl1.ResumeLayout(false);
        this.tabBrowse.ResumeLayout(false);
        this.tabBrowse.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvBrowseProperties)).EndInit();
        this.tabCart.ResumeLayout(false);
        this.tabCart.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
        this.tabOrders.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
        this.tabMyVisits.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvMyVisits)).EndInit();
        this.tabReviews.ResumeLayout(false);
        this.tabReviews.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numRating)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvReviews)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabBrowse;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label lblFilter;
    private System.Windows.Forms.ComboBox cmbListingTypeFilter;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.DataGridView dgvBrowseProperties;
    private System.Windows.Forms.Button btnAddToCart;
    private System.Windows.Forms.Button btnRequestVisit;
    private System.Windows.Forms.TabPage tabCart;
    private System.Windows.Forms.DataGridView dgvCart;
    private System.Windows.Forms.Button btnRemoveFromCart;
    private System.Windows.Forms.Button btnCheckout;
    private System.Windows.Forms.Label lblCartTotal;
    private System.Windows.Forms.TabPage tabOrders;
    private System.Windows.Forms.DataGridView dgvOrders;
    private System.Windows.Forms.TabPage tabMyVisits;
    private System.Windows.Forms.DataGridView dgvMyVisits;
    private System.Windows.Forms.TabPage tabReviews;
    private System.Windows.Forms.DataGridView dgvReviews;
    private System.Windows.Forms.Label lblSelectProperty;
    private System.Windows.Forms.ComboBox cmbReviewProperty;
    private System.Windows.Forms.Label lblRating;
    private System.Windows.Forms.NumericUpDown numRating;
    private System.Windows.Forms.Label lblComment;
    private System.Windows.Forms.TextBox txtReviewComment;
    private System.Windows.Forms.Button btnSubmitReview;
}
