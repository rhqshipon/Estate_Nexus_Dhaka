namespace EstateNexus;

partial class AddPropertyForm
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
        this.lblPropertyName = new System.Windows.Forms.Label();
        this.txtPropertyName = new System.Windows.Forms.TextBox();
        this.lblCategory = new System.Windows.Forms.Label();
        this.cmbCategory = new System.Windows.Forms.ComboBox();
        this.lblListingType = new System.Windows.Forms.Label();
        this.cmbListingType = new System.Windows.Forms.ComboBox();
        this.lblLocation = new System.Windows.Forms.Label();
        this.txtLocation = new System.Windows.Forms.TextBox();
        this.lblAddress = new System.Windows.Forms.Label();
        this.txtAddress = new System.Windows.Forms.TextBox();
        this.lblArea = new System.Windows.Forms.Label();
        this.txtArea = new System.Windows.Forms.TextBox();
        this.lblBedrooms = new System.Windows.Forms.Label();
        this.numBedrooms = new System.Windows.Forms.NumericUpDown();
        this.lblBathrooms = new System.Windows.Forms.Label();
        this.numBathrooms = new System.Windows.Forms.NumericUpDown();
        this.lblPrice = new System.Windows.Forms.Label();
        this.txtPrice = new System.Windows.Forms.TextBox();
        this.lblDescription = new System.Windows.Forms.Label();
        this.txtDescription = new System.Windows.Forms.TextBox();
        this.lblImage = new System.Windows.Forms.Label();
        this.btnChooseImage = new System.Windows.Forms.Button();
        this.lblImagePath = new System.Windows.Forms.Label();
        this.picImagePreview = new System.Windows.Forms.PictureBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.numBedrooms)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numBathrooms)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.picImagePreview)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(30, 20);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(200, 30);
        this.lblTitle.Text = "Add New Property";

        // lblPropertyName
        this.lblPropertyName.AutoSize = true;
        this.lblPropertyName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblPropertyName.Location = new System.Drawing.Point(30, 65);
        this.lblPropertyName.Name = "lblPropertyName";
        this.lblPropertyName.Size = new System.Drawing.Size(84, 17);
        this.lblPropertyName.Text = "Property Title:";

        // txtPropertyName
        this.txtPropertyName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtPropertyName.Location = new System.Drawing.Point(140, 62);
        this.txtPropertyName.Name = "txtPropertyName";
        this.txtPropertyName.Size = new System.Drawing.Size(370, 24);

        // lblCategory
        this.lblCategory.AutoSize = true;
        this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblCategory.Location = new System.Drawing.Point(30, 105);
        this.lblCategory.Name = "lblCategory";
        this.lblCategory.Size = new System.Drawing.Size(64, 17);
        this.lblCategory.Text = "Category:";

        // cmbCategory
        this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.cmbCategory.FormattingEnabled = true;
        this.cmbCategory.Location = new System.Drawing.Point(140, 102);
        this.cmbCategory.Name = "cmbCategory";
        this.cmbCategory.Size = new System.Drawing.Size(370, 24);

        // lblListingType
        this.lblListingType.AutoSize = true;
        this.lblListingType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblListingType.Location = new System.Drawing.Point(30, 145);
        this.lblListingType.Name = "lblListingType";
        this.lblListingType.Size = new System.Drawing.Size(78, 17);
        this.lblListingType.Text = "Listing Type:";

        // cmbListingType
        this.cmbListingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbListingType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.cmbListingType.FormattingEnabled = true;
        this.cmbListingType.Items.AddRange(new object[] { "Sale", "Rent" });
        this.cmbListingType.Location = new System.Drawing.Point(140, 142);
        this.cmbListingType.Name = "cmbListingType";
        this.cmbListingType.Size = new System.Drawing.Size(370, 24);

        // lblLocation
        this.lblLocation.AutoSize = true;
        this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblLocation.Location = new System.Drawing.Point(30, 185);
        this.lblLocation.Name = "lblLocation";
        this.lblLocation.Size = new System.Drawing.Size(60, 17);
        this.lblLocation.Text = "Location:";

        // txtLocation
        this.txtLocation.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtLocation.Location = new System.Drawing.Point(140, 182);
        this.txtLocation.Name = "txtLocation";
        this.txtLocation.Size = new System.Drawing.Size(370, 24);

        // lblAddress
        this.lblAddress.AutoSize = true;
        this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblAddress.Location = new System.Drawing.Point(30, 225);
        this.lblAddress.Name = "lblAddress";
        this.lblAddress.Size = new System.Drawing.Size(83, 17);
        this.lblAddress.Text = "Full Address:";

        // txtAddress
        this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtAddress.Location = new System.Drawing.Point(140, 222);
        this.txtAddress.Name = "txtAddress";
        this.txtAddress.Size = new System.Drawing.Size(370, 24);

        // lblArea
        this.lblArea.AutoSize = true;
        this.lblArea.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblArea.Location = new System.Drawing.Point(30, 265);
        this.lblArea.Name = "lblArea";
        this.lblArea.Size = new System.Drawing.Size(73, 17);
        this.lblArea.Text = "Area (sqft):";

        // txtArea
        this.txtArea.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtArea.Location = new System.Drawing.Point(140, 262);
        this.txtArea.Name = "txtArea";
        this.txtArea.Size = new System.Drawing.Size(100, 24);

        // lblBedrooms
        this.lblBedrooms.AutoSize = true;
        this.lblBedrooms.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblBedrooms.Location = new System.Drawing.Point(260, 265);
        this.lblBedrooms.Name = "lblBedrooms";
        this.lblBedrooms.Size = new System.Drawing.Size(40, 17);
        this.lblBedrooms.Text = "Beds:";

        // numBedrooms
        this.numBedrooms.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.numBedrooms.Location = new System.Drawing.Point(310, 262);
        this.numBedrooms.Name = "numBedrooms";
        this.numBedrooms.Size = new System.Drawing.Size(55, 24);

        // lblBathrooms
        this.lblBathrooms.AutoSize = true;
        this.lblBathrooms.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblBathrooms.Location = new System.Drawing.Point(390, 265);
        this.lblBathrooms.Name = "lblBathrooms";
        this.lblBathrooms.Size = new System.Drawing.Size(42, 17);
        this.lblBathrooms.Text = "Baths:";

        // numBathrooms
        this.numBathrooms.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.numBathrooms.Location = new System.Drawing.Point(445, 262);
        this.numBathrooms.Name = "numBathrooms";
        this.numBathrooms.Size = new System.Drawing.Size(65, 24);

        // lblPrice
        this.lblPrice.AutoSize = true;
        this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblPrice.Location = new System.Drawing.Point(30, 305);
        this.lblPrice.Name = "lblPrice";
        this.lblPrice.Size = new System.Drawing.Size(60, 17);
        this.lblPrice.Text = "Price (৳):";

        // txtPrice
        this.txtPrice.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtPrice.Location = new System.Drawing.Point(140, 302);
        this.txtPrice.Name = "txtPrice";
        this.txtPrice.Size = new System.Drawing.Size(370, 24);

        // lblDescription
        this.lblDescription.AutoSize = true;
        this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblDescription.Location = new System.Drawing.Point(30, 345);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.Size = new System.Drawing.Size(77, 17);
        this.lblDescription.Text = "Description:";

        // txtDescription
        this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.txtDescription.Location = new System.Drawing.Point(140, 342);
        this.txtDescription.Multiline = true;
        this.txtDescription.Name = "txtDescription";
        this.txtDescription.Size = new System.Drawing.Size(370, 65);

        // lblImage
        this.lblImage.AutoSize = true;
        this.lblImage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblImage.Location = new System.Drawing.Point(30, 420);
        this.lblImage.Name = "lblImage";
        this.lblImage.Size = new System.Drawing.Size(46, 17);
        this.lblImage.Text = "Image:";

        // btnChooseImage
        this.btnChooseImage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.btnChooseImage.Location = new System.Drawing.Point(140, 415);
        this.btnChooseImage.Name = "btnChooseImage";
        this.btnChooseImage.Size = new System.Drawing.Size(120, 30);
        this.btnChooseImage.Text = "Choose Image";
        this.btnChooseImage.UseVisualStyleBackColor = true;
        this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);

        // lblImagePath
        this.lblImagePath.AutoSize = true;
        this.lblImagePath.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblImagePath.ForeColor = System.Drawing.Color.DimGray;
        this.lblImagePath.Location = new System.Drawing.Point(270, 422);
        this.lblImagePath.Name = "lblImagePath";
        this.lblImagePath.Size = new System.Drawing.Size(110, 15);
        this.lblImagePath.Text = "No image selected";

        // picImagePreview
        this.picImagePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.picImagePreview.Location = new System.Drawing.Point(140, 452);
        this.picImagePreview.Name = "picImagePreview";
        this.picImagePreview.Size = new System.Drawing.Size(150, 95);
        this.picImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.picImagePreview.TabIndex = 24;
        this.picImagePreview.TabStop = false;

        // btnSave
        this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.btnSave.Location = new System.Drawing.Point(140, 560);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(170, 36);
        this.btnSave.Text = "Save Property";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        // btnCancel
        this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.btnCancel.Location = new System.Drawing.Point(340, 560);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(170, 36);
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        // AddPropertyForm
        this.ClientSize = new System.Drawing.Size(560, 615);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblPropertyName);
        this.Controls.Add(this.txtPropertyName);
        this.Controls.Add(this.lblCategory);
        this.Controls.Add(this.cmbCategory);
        this.Controls.Add(this.lblListingType);
        this.Controls.Add(this.cmbListingType);
        this.Controls.Add(this.lblLocation);
        this.Controls.Add(this.txtLocation);
        this.Controls.Add(this.lblAddress);
        this.Controls.Add(this.txtAddress);
        this.Controls.Add(this.lblArea);
        this.Controls.Add(this.txtArea);
        this.Controls.Add(this.lblBedrooms);
        this.Controls.Add(this.numBedrooms);
        this.Controls.Add(this.lblBathrooms);
        this.Controls.Add(this.numBathrooms);
        this.Controls.Add(this.lblPrice);
        this.Controls.Add(this.txtPrice);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.txtDescription);
        this.Controls.Add(this.lblImage);
        this.Controls.Add(this.btnChooseImage);
        this.Controls.Add(this.lblImagePath);
        this.Controls.Add(this.picImagePreview);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Add Property - EstateNexus";
        this.Load += new System.EventHandler(this.AddPropertyForm_Load);
        ((System.ComponentModel.ISupportInitialize)(this.numBedrooms)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numBathrooms)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.picImagePreview)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblPropertyName;
    private System.Windows.Forms.TextBox txtPropertyName;
    private System.Windows.Forms.Label lblCategory;
    private System.Windows.Forms.ComboBox cmbCategory;
    private System.Windows.Forms.Label lblListingType;
    private System.Windows.Forms.ComboBox cmbListingType;
    private System.Windows.Forms.Label lblLocation;
    private System.Windows.Forms.TextBox txtLocation;
    private System.Windows.Forms.Label lblAddress;
    private System.Windows.Forms.TextBox txtAddress;
    private System.Windows.Forms.Label lblArea;
    private System.Windows.Forms.TextBox txtArea;
    private System.Windows.Forms.Label lblBedrooms;
    private System.Windows.Forms.NumericUpDown numBedrooms;
    private System.Windows.Forms.Label lblBathrooms;
    private System.Windows.Forms.NumericUpDown numBathrooms;
    private System.Windows.Forms.Label lblPrice;
    private System.Windows.Forms.TextBox txtPrice;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblImage;
    private System.Windows.Forms.Button btnChooseImage;
    private System.Windows.Forms.Label lblImagePath;
    private System.Windows.Forms.PictureBox picImagePreview;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
