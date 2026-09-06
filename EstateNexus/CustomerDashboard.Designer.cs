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
        this.lblCategoryFilter = new System.Windows.Forms.Label();
        this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
        this.lblDistrictFilter = new System.Windows.Forms.Label();
        this.cmbDistrictFilter = new System.Windows.Forms.ComboBox();
        this.lblPriceFilter = new System.Windows.Forms.Label();
        this.cmbPriceFilter = new System.Windows.Forms.ComboBox();
        this.lblBedroomsFilter = new System.Windows.Forms.Label();
        this.cmbBedroomsFilter = new System.Windows.Forms.ComboBox();
        this.txtSearch = new System.Windows.Forms.TextBox();
        this.btnSearch = new System.Windows.Forms.Button();
        this.btnApplyFilters = new System.Windows.Forms.Button();
        this.btnResetFilters = new System.Windows.Forms.Button();
        this.lblResultCount = new System.Windows.Forms.Label();
        this.dgvBrowseProperties = new System.Windows.Forms.DataGridView();
        this.btnAddToCart = new System.Windows.Forms.Button();
        this.btnRequestVisit = new System.Windows.Forms.Button();
        this.tabCart = new System.Windows.Forms.TabPage();
        this.lblCartTotal = new System.Windows.Forms.Label();
        this.btnRemoveFromCart = new System.Windows.Forms.Button();
        this.dgvCart = new System.Windows.Forms.DataGridView();
        this.btnCheckout = new System.Windows.Forms.Button();
        this.lblPaymentMethod = new System.Windows.Forms.Label();
        this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
        this.tabOrders = new System.Windows.Forms.TabPage();
        this.dgvOrders = new System.Windows.Forms.DataGridView();
        this.tabMyVisits = new System.Windows.Forms.TabPage();
        this.dgvMyVisits = new System.Windows.Forms.DataGridView();
        this.btnRefreshVisits = new System.Windows.Forms.Button();
        this.btnCancelVisit = new System.Windows.Forms.Button();
        this.lblVisitStatusSummary = new System.Windows.Forms.Label();
        this.tabReviews = new System.Windows.Forms.TabPage();
        this.lblSelectProperty = new System.Windows.Forms.Label();
        this.cmbReviewProperty = new System.Windows.Forms.ComboBox();
        this.lblRating = new System.Windows.Forms.Label();
        this.numRating = new System.Windows.Forms.NumericUpDown();
        this.lblComment = new System.Windows.Forms.Label();
        this.txtReviewComment = new System.Windows.Forms.TextBox();
        this.btnSubmitReview = new System.Windows.Forms.Button();
        this.dgvReviews = new System.Windows.Forms.DataGridView();

        this.lblRentalMonths = new System.Windows.Forms.Label();
        this.numRentalMonths = new System.Windows.Forms.NumericUpDown();
        this.tabProfile = new System.Windows.Forms.TabPage();
        this.lblProfileHeader = new System.Windows.Forms.Label();
        this.lblProfileFullName = new System.Windows.Forms.Label();
        this.txtProfileFullName = new System.Windows.Forms.TextBox();
        this.lblProfileEmail = new System.Windows.Forms.Label();
        this.txtProfileEmail = new System.Windows.Forms.TextBox();
        this.lblProfilePhone = new System.Windows.Forms.Label();
        this.txtProfilePhone = new System.Windows.Forms.TextBox();
        this.lblProfileAddress = new System.Windows.Forms.Label();
        this.txtProfileAddress = new System.Windows.Forms.TextBox();
        this.lblProfileImagePath = new System.Windows.Forms.Label();
        this.txtProfileImagePath = new System.Windows.Forms.TextBox();
        this.btnBrowseProfileImage = new System.Windows.Forms.Button();
        this.picProfilePreview = new System.Windows.Forms.PictureBox();
        this.btnSaveProfile = new System.Windows.Forms.Button();

        this.tabControl1.SuspendLayout();
        this.tabBrowse.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvBrowseProperties)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numRentalMonths)).BeginInit();
        this.tabCart.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
        this.tabOrders.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
        this.tabMyVisits.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMyVisits)).BeginInit();
        this.tabReviews.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numRating)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvReviews)).BeginInit();
        this.tabProfile.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picProfilePreview)).BeginInit();
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
        this.tabControl1.Controls.Add(this.tabProfile);
        this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.tabControl1.Location = new System.Drawing.Point(15, 55);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(930, 495);
        this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);

        // tabBrowse
        this.tabBrowse.Controls.Add(this.lblFilter);
        this.tabBrowse.Controls.Add(this.cmbListingTypeFilter);
        this.tabBrowse.Controls.Add(this.lblCategoryFilter);
        this.tabBrowse.Controls.Add(this.cmbCategoryFilter);
        this.tabBrowse.Controls.Add(this.lblDistrictFilter);
        this.tabBrowse.Controls.Add(this.cmbDistrictFilter);
        this.tabBrowse.Controls.Add(this.lblPriceFilter);
        this.tabBrowse.Controls.Add(this.cmbPriceFilter);
        this.tabBrowse.Controls.Add(this.lblBedroomsFilter);
        this.tabBrowse.Controls.Add(this.cmbBedroomsFilter);
        this.tabBrowse.Controls.Add(this.txtSearch);
        this.tabBrowse.Controls.Add(this.btnSearch);
        this.tabBrowse.Controls.Add(this.btnApplyFilters);
        this.tabBrowse.Controls.Add(this.btnResetFilters);
        this.tabBrowse.Controls.Add(this.lblResultCount);
        this.tabBrowse.Controls.Add(this.dgvBrowseProperties);
        this.tabBrowse.Controls.Add(this.btnAddToCart);
        this.tabBrowse.Controls.Add(this.btnRequestVisit);
        this.tabBrowse.Controls.Add(this.lblRentalMonths);
        this.tabBrowse.Controls.Add(this.numRentalMonths);
        this.tabBrowse.Location = new System.Drawing.Point(4, 26);
        this.tabBrowse.Name = "tabBrowse";
        this.tabBrowse.Padding = new System.Windows.Forms.Padding(3);
        this.tabBrowse.Size = new System.Drawing.Size(922, 465);
        this.tabBrowse.Text = "Browse Properties";
        this.tabBrowse.UseVisualStyleBackColor = true;

        // txtSearch
        this.txtSearch.Location = new System.Drawing.Point(8, 8);
        this.txtSearch.Name = "txtSearch";
        this.txtSearch.PlaceholderText = "Search title/location...";
        this.txtSearch.Size = new System.Drawing.Size(165, 24);

        // lblFilter
        this.lblFilter.AutoSize = true;
        this.lblFilter.Location = new System.Drawing.Point(177, 11);
        this.lblFilter.Name = "lblFilter";
        this.lblFilter.Size = new System.Drawing.Size(38, 17);
        this.lblFilter.Text = "Type:";

        // cmbListingTypeFilter
        this.cmbListingTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbListingTypeFilter.FormattingEnabled = true;
        this.cmbListingTypeFilter.Items.AddRange(new object[] { "All", "Sale", "Rent" });
        this.cmbListingTypeFilter.Location = new System.Drawing.Point(217, 8);
        this.cmbListingTypeFilter.Name = "cmbListingTypeFilter";
        this.cmbListingTypeFilter.Size = new System.Drawing.Size(65, 25);

        // lblCategoryFilter
        this.lblCategoryFilter.AutoSize = true;
        this.lblCategoryFilter.Location = new System.Drawing.Point(286, 11);
        this.lblCategoryFilter.Name = "lblCategoryFilter";
        this.lblCategoryFilter.Size = new System.Drawing.Size(31, 17);
        this.lblCategoryFilter.Text = "Cat:";

        // cmbCategoryFilter
        this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbCategoryFilter.FormattingEnabled = true;
        this.cmbCategoryFilter.Location = new System.Drawing.Point(320, 8);
        this.cmbCategoryFilter.Name = "cmbCategoryFilter";
        this.cmbCategoryFilter.Size = new System.Drawing.Size(105, 25);

        // lblDistrictFilter
        this.lblDistrictFilter.AutoSize = true;
        this.lblDistrictFilter.Location = new System.Drawing.Point(430, 11);
        this.lblDistrictFilter.Name = "lblDistrictFilter";
        this.lblDistrictFilter.Size = new System.Drawing.Size(36, 17);
        this.lblDistrictFilter.Text = "Dist:";

        // cmbDistrictFilter
        this.cmbDistrictFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbDistrictFilter.FormattingEnabled = true;
        this.cmbDistrictFilter.Location = new System.Drawing.Point(468, 8);
        this.cmbDistrictFilter.Name = "cmbDistrictFilter";
        this.cmbDistrictFilter.Size = new System.Drawing.Size(95, 25);

        // lblPriceFilter
        this.lblPriceFilter.AutoSize = true;
        this.lblPriceFilter.Location = new System.Drawing.Point(568, 11);
        this.lblPriceFilter.Name = "lblPriceFilter";
        this.lblPriceFilter.Size = new System.Drawing.Size(39, 17);
        this.lblPriceFilter.Text = "Price:";

        // cmbPriceFilter
        this.cmbPriceFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbPriceFilter.FormattingEnabled = true;
        this.cmbPriceFilter.Items.AddRange(new object[] {
            "All",
            "Under 20,000",
            "20,000 - 50,000",
            "50,000 - 1,00,000",
            "1,00,000 - 50,00,000",
            "Above 50,00,000"
        });
        this.cmbPriceFilter.Location = new System.Drawing.Point(610, 8);
        this.cmbPriceFilter.Name = "cmbPriceFilter";
        this.cmbPriceFilter.Size = new System.Drawing.Size(140, 25);

        // lblBedroomsFilter
        this.lblBedroomsFilter.AutoSize = true;
        this.lblBedroomsFilter.Location = new System.Drawing.Point(755, 11);
        this.lblBedroomsFilter.Name = "lblBedroomsFilter";
        this.lblBedroomsFilter.Size = new System.Drawing.Size(40, 17);
        this.lblBedroomsFilter.Text = "Beds:";

        // cmbBedroomsFilter
        this.cmbBedroomsFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbBedroomsFilter.FormattingEnabled = true;
        this.cmbBedroomsFilter.Items.AddRange(new object[] { "All", "1", "2", "3", "4+" });
        this.cmbBedroomsFilter.Location = new System.Drawing.Point(798, 8);
        this.cmbBedroomsFilter.Name = "cmbBedroomsFilter";
        this.cmbBedroomsFilter.Size = new System.Drawing.Size(55, 25);

        // btnApplyFilters
        this.btnApplyFilters.Location = new System.Drawing.Point(8, 38);
        this.btnApplyFilters.Name = "btnApplyFilters";
        this.btnApplyFilters.Size = new System.Drawing.Size(80, 27);
        this.btnApplyFilters.Text = "Apply";
        this.btnApplyFilters.UseVisualStyleBackColor = true;
        this.btnApplyFilters.Click += new System.EventHandler(this.btnApplyFilters_Click);

        // btnResetFilters
        this.btnResetFilters.Location = new System.Drawing.Point(95, 38);
        this.btnResetFilters.Name = "btnResetFilters";
        this.btnResetFilters.Size = new System.Drawing.Size(80, 27);
        this.btnResetFilters.Text = "Reset";
        this.btnResetFilters.UseVisualStyleBackColor = true;
        this.btnResetFilters.Click += new System.EventHandler(this.btnResetFilters_Click);

        // btnSearch
        this.btnSearch.Location = new System.Drawing.Point(860, 7);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(55, 27);
        this.btnSearch.Text = "Go";
        this.btnSearch.UseVisualStyleBackColor = true;
        this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

        // lblResultCount
        this.lblResultCount.AutoSize = true;
        this.lblResultCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblResultCount.Location = new System.Drawing.Point(190, 43);
        this.lblResultCount.Name = "lblResultCount";
        this.lblResultCount.Size = new System.Drawing.Size(125, 17);
        this.lblResultCount.Text = "0 properties found";

        // dgvBrowseProperties
        this.dgvBrowseProperties.AllowUserToAddRows = false;
        this.dgvBrowseProperties.AllowUserToDeleteRows = false;
        this.dgvBrowseProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvBrowseProperties.Location = new System.Drawing.Point(8, 70);
        this.dgvBrowseProperties.MultiSelect = false;
        this.dgvBrowseProperties.Name = "dgvBrowseProperties";
        this.dgvBrowseProperties.ReadOnly = true;
        this.dgvBrowseProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvBrowseProperties.Size = new System.Drawing.Size(905, 340);

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

        // lblRentalMonths
        this.lblRentalMonths.AutoSize = true;
        this.lblRentalMonths.Location = new System.Drawing.Point(360, 428);
        this.lblRentalMonths.Name = "lblRentalMonths";
        this.lblRentalMonths.Size = new System.Drawing.Size(107, 17);
        this.lblRentalMonths.Text = "Months (if Rent):";

        // numRentalMonths
        this.numRentalMonths.Location = new System.Drawing.Point(475, 425);
        this.numRentalMonths.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
        this.numRentalMonths.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numRentalMonths.Name = "numRentalMonths";
        this.numRentalMonths.Size = new System.Drawing.Size(65, 24);
        this.numRentalMonths.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // tabCart
        this.tabCart.Controls.Add(this.lblCartTotal);
        this.tabCart.Controls.Add(this.lblPaymentMethod);
        this.tabCart.Controls.Add(this.cmbPaymentMethod);
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
        this.btnRemoveFromCart.Size = new System.Drawing.Size(125, 35);
        this.btnRemoveFromCart.Text = "Remove Item";
        this.btnRemoveFromCart.UseVisualStyleBackColor = true;
        this.btnRemoveFromCart.Click += new System.EventHandler(this.btnRemoveFromCart_Click);

        // lblPaymentMethod
        this.lblPaymentMethod.AutoSize = true;
        this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblPaymentMethod.Location = new System.Drawing.Point(145, 428);
        this.lblPaymentMethod.Name = "lblPaymentMethod";
        this.lblPaymentMethod.Size = new System.Drawing.Size(117, 17);
        this.lblPaymentMethod.Text = "Payment Method:";

        // cmbPaymentMethod
        this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.cmbPaymentMethod.FormattingEnabled = true;
        this.cmbPaymentMethod.Items.AddRange(new object[] {
            "Card",
            "bKash",
            "Nagad",
            "Bank Transfer"});
        this.cmbPaymentMethod.Location = new System.Drawing.Point(268, 425);
        this.cmbPaymentMethod.Name = "cmbPaymentMethod";
        this.cmbPaymentMethod.Size = new System.Drawing.Size(130, 25);

        // btnCheckout
        this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnCheckout.Location = new System.Drawing.Point(410, 420);
        this.btnCheckout.Name = "btnCheckout";
        this.btnCheckout.Size = new System.Drawing.Size(175, 35);
        this.btnCheckout.Text = "Proceed to Checkout";
        this.btnCheckout.UseVisualStyleBackColor = true;
        this.btnCheckout.Click += new System.EventHandler(this.btnCheckout_Click);

        // lblCartTotal
        this.lblCartTotal.AutoSize = true;
        this.lblCartTotal.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
        this.lblCartTotal.ForeColor = System.Drawing.Color.DarkGreen;
        this.lblCartTotal.Location = new System.Drawing.Point(600, 427);
        this.lblCartTotal.Name = "lblCartTotal";
        this.lblCartTotal.Size = new System.Drawing.Size(163, 21);
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
        this.tabMyVisits.Controls.Add(this.btnRefreshVisits);
        this.tabMyVisits.Controls.Add(this.btnCancelVisit);
        this.tabMyVisits.Controls.Add(this.lblVisitStatusSummary);
        this.tabMyVisits.Controls.Add(this.dgvMyVisits);
        this.tabMyVisits.Location = new System.Drawing.Point(4, 26);
        this.tabMyVisits.Name = "tabMyVisits";
        this.tabMyVisits.Padding = new System.Windows.Forms.Padding(3);
        this.tabMyVisits.Size = new System.Drawing.Size(922, 465);
        this.tabMyVisits.Text = "My Visit Requests";
        this.tabMyVisits.UseVisualStyleBackColor = true;

        // btnRefreshVisits
        this.btnRefreshVisits.Location = new System.Drawing.Point(8, 8);
        this.btnRefreshVisits.Name = "btnRefreshVisits";
        this.btnRefreshVisits.Size = new System.Drawing.Size(120, 32);
        this.btnRefreshVisits.Text = "Refresh Visits";
        this.btnRefreshVisits.UseVisualStyleBackColor = true;
        this.btnRefreshVisits.Click += new System.EventHandler(this.btnRefreshVisits_Click);

        // btnCancelVisit
        this.btnCancelVisit.Location = new System.Drawing.Point(138, 8);
        this.btnCancelVisit.Name = "btnCancelVisit";
        this.btnCancelVisit.Size = new System.Drawing.Size(140, 32);
        this.btnCancelVisit.Text = "Cancel Request";
        this.btnCancelVisit.UseVisualStyleBackColor = true;
        this.btnCancelVisit.Click += new System.EventHandler(this.btnCancelVisit_Click);

        // lblVisitStatusSummary
        this.lblVisitStatusSummary.AutoSize = true;
        this.lblVisitStatusSummary.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblVisitStatusSummary.ForeColor = System.Drawing.Color.DimGray;
        this.lblVisitStatusSummary.Location = new System.Drawing.Point(295, 15);
        this.lblVisitStatusSummary.Name = "lblVisitStatusSummary";
        this.lblVisitStatusSummary.Size = new System.Drawing.Size(60, 17);
        this.lblVisitStatusSummary.Text = "Total: 0";

        // dgvMyVisits
        this.dgvMyVisits.AllowUserToAddRows = false;
        this.dgvMyVisits.AllowUserToDeleteRows = false;
        this.dgvMyVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvMyVisits.Location = new System.Drawing.Point(8, 48);
        this.dgvMyVisits.MultiSelect = false;
        this.dgvMyVisits.Name = "dgvMyVisits";
        this.dgvMyVisits.ReadOnly = true;
        this.dgvMyVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvMyVisits.Size = new System.Drawing.Size(905, 405);

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

        // tabProfile
        this.tabProfile.Controls.Add(this.lblProfileHeader);
        this.tabProfile.Controls.Add(this.lblProfileFullName);
        this.tabProfile.Controls.Add(this.txtProfileFullName);
        this.tabProfile.Controls.Add(this.lblProfileEmail);
        this.tabProfile.Controls.Add(this.txtProfileEmail);
        this.tabProfile.Controls.Add(this.lblProfilePhone);
        this.tabProfile.Controls.Add(this.txtProfilePhone);
        this.tabProfile.Controls.Add(this.lblProfileAddress);
        this.tabProfile.Controls.Add(this.txtProfileAddress);
        this.tabProfile.Controls.Add(this.lblProfileImagePath);
        this.tabProfile.Controls.Add(this.txtProfileImagePath);
        this.tabProfile.Controls.Add(this.btnBrowseProfileImage);
        this.tabProfile.Controls.Add(this.picProfilePreview);
        this.tabProfile.Controls.Add(this.btnSaveProfile);
        this.tabProfile.Location = new System.Drawing.Point(4, 26);
        this.tabProfile.Name = "tabProfile";
        this.tabProfile.Padding = new System.Windows.Forms.Padding(3);
        this.tabProfile.Size = new System.Drawing.Size(922, 465);
        this.tabProfile.Text = "My Profile & Settings";
        this.tabProfile.UseVisualStyleBackColor = true;

        // lblProfileHeader
        this.lblProfileHeader.AutoSize = true;
        this.lblProfileHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
        this.lblProfileHeader.Location = new System.Drawing.Point(20, 20);
        this.lblProfileHeader.Name = "lblProfileHeader";
        this.lblProfileHeader.Size = new System.Drawing.Size(250, 25);
        this.lblProfileHeader.Text = "User Profile & Settings";

        // lblProfileFullName
        this.lblProfileFullName.AutoSize = true;
        this.lblProfileFullName.Location = new System.Drawing.Point(20, 70);
        this.lblProfileFullName.Name = "lblProfileFullName";
        this.lblProfileFullName.Size = new System.Drawing.Size(69, 17);
        this.lblProfileFullName.Text = "Full Name:";

        // txtProfileFullName
        this.txtProfileFullName.Location = new System.Drawing.Point(150, 67);
        this.txtProfileFullName.Name = "txtProfileFullName";
        this.txtProfileFullName.Size = new System.Drawing.Size(320, 24);

        // lblProfileEmail
        this.lblProfileEmail.AutoSize = true;
        this.lblProfileEmail.Location = new System.Drawing.Point(20, 110);
        this.lblProfileEmail.Name = "lblProfileEmail";
        this.lblProfileEmail.Size = new System.Drawing.Size(42, 17);
        this.lblProfileEmail.Text = "Email:";

        // txtProfileEmail
        this.txtProfileEmail.Location = new System.Drawing.Point(150, 107);
        this.txtProfileEmail.Name = "txtProfileEmail";
        this.txtProfileEmail.ReadOnly = true;
        this.txtProfileEmail.Size = new System.Drawing.Size(320, 24);

        // lblProfilePhone
        this.lblProfilePhone.AutoSize = true;
        this.lblProfilePhone.Location = new System.Drawing.Point(20, 150);
        this.lblProfilePhone.Name = "lblProfilePhone";
        this.lblProfilePhone.Size = new System.Drawing.Size(47, 17);
        this.lblProfilePhone.Text = "Phone:";

        // txtProfilePhone
        this.txtProfilePhone.Location = new System.Drawing.Point(150, 147);
        this.txtProfilePhone.Name = "txtProfilePhone";
        this.txtProfilePhone.Size = new System.Drawing.Size(320, 24);

        // lblProfileAddress
        this.lblProfileAddress.AutoSize = true;
        this.lblProfileAddress.Location = new System.Drawing.Point(20, 190);
        this.lblProfileAddress.Name = "lblProfileAddress";
        this.lblProfileAddress.Size = new System.Drawing.Size(59, 17);
        this.lblProfileAddress.Text = "Address:";

        // txtProfileAddress
        this.txtProfileAddress.Location = new System.Drawing.Point(150, 187);
        this.txtProfileAddress.Name = "txtProfileAddress";
        this.txtProfileAddress.Size = new System.Drawing.Size(320, 24);

        // lblProfileImagePath
        this.lblProfileImagePath.AutoSize = true;
        this.lblProfileImagePath.Location = new System.Drawing.Point(20, 230);
        this.lblProfileImagePath.Name = "lblProfileImagePath";
        this.lblProfileImagePath.Size = new System.Drawing.Size(117, 17);
        this.lblProfileImagePath.Text = "Profile Image Path:";

        // txtProfileImagePath
        this.txtProfileImagePath.Location = new System.Drawing.Point(150, 227);
        this.txtProfileImagePath.Name = "txtProfileImagePath";
        this.txtProfileImagePath.Size = new System.Drawing.Size(320, 24);

        // btnBrowseProfileImage
        this.btnBrowseProfileImage.Location = new System.Drawing.Point(480, 226);
        this.btnBrowseProfileImage.Name = "btnBrowseProfileImage";
        this.btnBrowseProfileImage.Size = new System.Drawing.Size(85, 27);
        this.btnBrowseProfileImage.Text = "Browse...";
        this.btnBrowseProfileImage.UseVisualStyleBackColor = true;
        this.btnBrowseProfileImage.Click += new System.EventHandler(this.btnBrowseProfileImage_Click);

        // picProfilePreview
        this.picProfilePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picProfilePreview.Location = new System.Drawing.Point(590, 67);
        this.picProfilePreview.Name = "picProfilePreview";
        this.picProfilePreview.Size = new System.Drawing.Size(180, 180);
        this.picProfilePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picProfilePreview.TabStop = false;

        // btnSaveProfile
        this.btnSaveProfile.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.btnSaveProfile.Location = new System.Drawing.Point(150, 280);
        this.btnSaveProfile.Name = "btnSaveProfile";
        this.btnSaveProfile.Size = new System.Drawing.Size(180, 35);
        this.btnSaveProfile.Text = "Save Profile Settings";
        this.btnSaveProfile.UseVisualStyleBackColor = true;
        this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);

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
        ((System.ComponentModel.ISupportInitialize)(this.numRentalMonths)).EndInit();
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
        this.tabProfile.ResumeLayout(false);
        this.tabProfile.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.picProfilePreview)).EndInit();
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
    private System.Windows.Forms.Label lblCategoryFilter;
    private System.Windows.Forms.ComboBox cmbCategoryFilter;
    private System.Windows.Forms.Label lblDistrictFilter;
    private System.Windows.Forms.ComboBox cmbDistrictFilter;
    private System.Windows.Forms.Label lblPriceFilter;
    private System.Windows.Forms.ComboBox cmbPriceFilter;
    private System.Windows.Forms.Label lblBedroomsFilter;
    private System.Windows.Forms.ComboBox cmbBedroomsFilter;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.Button btnApplyFilters;
    private System.Windows.Forms.Button btnResetFilters;
    private System.Windows.Forms.Label lblResultCount;
    private System.Windows.Forms.DataGridView dgvBrowseProperties;
    private System.Windows.Forms.Button btnAddToCart;
    private System.Windows.Forms.Button btnRequestVisit;
    private System.Windows.Forms.Label lblRentalMonths;
    private System.Windows.Forms.NumericUpDown numRentalMonths;
    private System.Windows.Forms.TabPage tabCart;
    private System.Windows.Forms.DataGridView dgvCart;
    private System.Windows.Forms.Button btnRemoveFromCart;
    private System.Windows.Forms.Button btnCheckout;
    private System.Windows.Forms.Label lblPaymentMethod;
    private System.Windows.Forms.ComboBox cmbPaymentMethod;
    private System.Windows.Forms.Label lblCartTotal;
    private System.Windows.Forms.TabPage tabOrders;
    private System.Windows.Forms.DataGridView dgvOrders;
    private System.Windows.Forms.TabPage tabMyVisits;
    private System.Windows.Forms.DataGridView dgvMyVisits;
    private System.Windows.Forms.Button btnRefreshVisits;
    private System.Windows.Forms.Button btnCancelVisit;
    private System.Windows.Forms.Label lblVisitStatusSummary;
    private System.Windows.Forms.TabPage tabReviews;
    private System.Windows.Forms.DataGridView dgvReviews;
    private System.Windows.Forms.Label lblSelectProperty;
    private System.Windows.Forms.ComboBox cmbReviewProperty;
    private System.Windows.Forms.Label lblRating;
    private System.Windows.Forms.NumericUpDown numRating;
    private System.Windows.Forms.Label lblComment;
    private System.Windows.Forms.TextBox txtReviewComment;
    private System.Windows.Forms.Button btnSubmitReview;
    private System.Windows.Forms.TabPage tabProfile;
    private System.Windows.Forms.Label lblProfileHeader;
    private System.Windows.Forms.Label lblProfileFullName;
    private System.Windows.Forms.TextBox txtProfileFullName;
    private System.Windows.Forms.Label lblProfileEmail;
    private System.Windows.Forms.TextBox txtProfileEmail;
    private System.Windows.Forms.Label lblProfilePhone;
    private System.Windows.Forms.TextBox txtProfilePhone;
    private System.Windows.Forms.Label lblProfileAddress;
    private System.Windows.Forms.TextBox txtProfileAddress;
    private System.Windows.Forms.Label lblProfileImagePath;
    private System.Windows.Forms.TextBox txtProfileImagePath;
    private System.Windows.Forms.Button btnBrowseProfileImage;
    private System.Windows.Forms.PictureBox picProfilePreview;
    private System.Windows.Forms.Button btnSaveProfile;
}
