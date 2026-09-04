namespace EstateNexus;

partial class ScheduleVisitForm
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
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.lblHeaderSubtitle = new System.Windows.Forms.Label();
        this.lblHeaderTitle = new System.Windows.Forms.Label();
        this.grpPropertyInfo = new System.Windows.Forms.GroupBox();
        this.lblPropertyLocationVal = new System.Windows.Forms.Label();
        this.lblPropertyLocation = new System.Windows.Forms.Label();
        this.lblPropertyTitleVal = new System.Windows.Forms.Label();
        this.lblPropertyTitle = new System.Windows.Forms.Label();
        this.lblVisitDate = new System.Windows.Forms.Label();
        this.dtpVisitDate = new System.Windows.Forms.DateTimePicker();
        this.lblVisitTime = new System.Windows.Forms.Label();
        this.cmbVisitTime = new System.Windows.Forms.ComboBox();
        this.lblNotes = new System.Windows.Forms.Label();
        this.txtNotes = new System.Windows.Forms.TextBox();
        this.btnSubmit = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.pnlHeader.SuspendLayout();
        this.grpPropertyInfo.SuspendLayout();
        this.SuspendLayout();

        // pnlHeader
        this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
        this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
        this.pnlHeader.Controls.Add(this.lblHeaderTitle);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(464, 65);
        this.pnlHeader.TabIndex = 0;

        // lblHeaderTitle
        this.lblHeaderTitle.AutoSize = true;
        this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
        this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
        this.lblHeaderTitle.Location = new System.Drawing.Point(16, 10);
        this.lblHeaderTitle.Name = "lblHeaderTitle";
        this.lblHeaderTitle.Size = new System.Drawing.Size(206, 25);
        this.lblHeaderTitle.Text = "Schedule Property Visit";

        // lblHeaderSubtitle
        this.lblHeaderSubtitle.AutoSize = true;
        this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
        this.lblHeaderSubtitle.Location = new System.Drawing.Point(18, 37);
        this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
        this.lblHeaderSubtitle.Size = new System.Drawing.Size(325, 15);
        this.lblHeaderSubtitle.Text = "Select your preferred date and time to arrange a guided property tour";

        // grpPropertyInfo
        this.grpPropertyInfo.Controls.Add(this.lblPropertyLocationVal);
        this.grpPropertyInfo.Controls.Add(this.lblPropertyLocation);
        this.grpPropertyInfo.Controls.Add(this.lblPropertyTitleVal);
        this.grpPropertyInfo.Controls.Add(this.lblPropertyTitle);
        this.grpPropertyInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.grpPropertyInfo.Location = new System.Drawing.Point(20, 75);
        this.grpPropertyInfo.Name = "grpPropertyInfo";
        this.grpPropertyInfo.Size = new System.Drawing.Size(424, 75);
        this.grpPropertyInfo.TabIndex = 1;
        this.grpPropertyInfo.TabStop = false;
        this.grpPropertyInfo.Text = "Selected Property";

        // lblPropertyTitle
        this.lblPropertyTitle.AutoSize = true;
        this.lblPropertyTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblPropertyTitle.ForeColor = System.Drawing.Color.DimGray;
        this.lblPropertyTitle.Location = new System.Drawing.Point(10, 22);
        this.lblPropertyTitle.Name = "lblPropertyTitle";
        this.lblPropertyTitle.Size = new System.Drawing.Size(33, 15);
        this.lblPropertyTitle.Text = "Title:";

        // lblPropertyTitleVal
        this.lblPropertyTitleVal.AutoEllipsis = true;
        this.lblPropertyTitleVal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblPropertyTitleVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
        this.lblPropertyTitleVal.Location = new System.Drawing.Point(70, 20);
        this.lblPropertyTitleVal.Name = "lblPropertyTitleVal";
        this.lblPropertyTitleVal.Size = new System.Drawing.Size(344, 20);
        this.lblPropertyTitleVal.Text = "Luxury Apartment";

        // lblPropertyLocation
        this.lblPropertyLocation.AutoSize = true;
        this.lblPropertyLocation.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblPropertyLocation.ForeColor = System.Drawing.Color.DimGray;
        this.lblPropertyLocation.Location = new System.Drawing.Point(10, 48);
        this.lblPropertyLocation.Name = "lblPropertyLocation";
        this.lblPropertyLocation.Size = new System.Drawing.Size(56, 15);
        this.lblPropertyLocation.Text = "Location:";

        // lblPropertyLocationVal
        this.lblPropertyLocationVal.AutoEllipsis = true;
        this.lblPropertyLocationVal.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblPropertyLocationVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
        this.lblPropertyLocationVal.Location = new System.Drawing.Point(70, 48);
        this.lblPropertyLocationVal.Name = "lblPropertyLocationVal";
        this.lblPropertyLocationVal.Size = new System.Drawing.Size(344, 18);
        this.lblPropertyLocationVal.Text = "Gulshan, Dhaka";

        // lblVisitDate
        this.lblVisitDate.AutoSize = true;
        this.lblVisitDate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblVisitDate.Location = new System.Drawing.Point(20, 162);
        this.lblVisitDate.Name = "lblVisitDate";
        this.lblVisitDate.Size = new System.Drawing.Size(135, 17);
        this.lblVisitDate.TabIndex = 2;
        this.lblVisitDate.Text = "Preferred Visit Date:*";

        // dtpVisitDate
        this.dtpVisitDate.CustomFormat = "yyyy-MM-dd (dddd)";
        this.dtpVisitDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.dtpVisitDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        this.dtpVisitDate.Location = new System.Drawing.Point(20, 185);
        this.dtpVisitDate.Name = "dtpVisitDate";
        this.dtpVisitDate.Size = new System.Drawing.Size(200, 24);
        this.dtpVisitDate.TabIndex = 3;

        // lblVisitTime
        this.lblVisitTime.AutoSize = true;
        this.lblVisitTime.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
        this.lblVisitTime.Location = new System.Drawing.Point(240, 162);
        this.lblVisitTime.Name = "lblVisitTime";
        this.lblVisitTime.Size = new System.Drawing.Size(135, 17);
        this.lblVisitTime.TabIndex = 4;
        this.lblVisitTime.Text = "Preferred Time Slot:*";

        // cmbVisitTime
        this.cmbVisitTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbVisitTime.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.cmbVisitTime.FormattingEnabled = true;
        this.cmbVisitTime.Items.AddRange(new object[] {
            "09:00 AM",
            "10:00 AM",
            "11:30 AM",
            "01:00 PM",
            "02:30 PM",
            "04:00 PM",
            "05:30 PM",
            "06:30 PM"
        });
        this.cmbVisitTime.Location = new System.Drawing.Point(240, 185);
        this.cmbVisitTime.Name = "cmbVisitTime";
        this.cmbVisitTime.Size = new System.Drawing.Size(204, 24);
        this.cmbVisitTime.TabIndex = 5;

        // lblNotes
        this.lblNotes.AutoSize = true;
        this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        this.lblNotes.Location = new System.Drawing.Point(20, 225);
        this.lblNotes.Name = "lblNotes";
        this.lblNotes.Size = new System.Drawing.Size(186, 17);
        this.lblNotes.TabIndex = 6;
        this.lblNotes.Text = "Customer Notes (Optional):";

        // txtNotes
        this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.txtNotes.Location = new System.Drawing.Point(20, 248);
        this.txtNotes.Multiline = true;
        this.txtNotes.Name = "txtNotes";
        this.txtNotes.PlaceholderText = "e.g., Interested in checking bedrooms, balcony and car parking.";
        this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtNotes.Size = new System.Drawing.Size(424, 60);
        this.txtNotes.TabIndex = 7;

        // btnSubmit
        this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
        this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.btnSubmit.ForeColor = System.Drawing.Color.White;
        this.btnSubmit.Location = new System.Drawing.Point(190, 325);
        this.btnSubmit.Name = "btnSubmit";
        this.btnSubmit.Size = new System.Drawing.Size(150, 36);
        this.btnSubmit.TabIndex = 8;
        this.btnSubmit.Text = "Submit Request";
        this.btnSubmit.UseVisualStyleBackColor = false;
        this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);

        // btnCancel
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.btnCancel.Location = new System.Drawing.Point(349, 325);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(95, 36);
        this.btnCancel.TabIndex = 9;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;

        // ScheduleVisitForm
        this.AcceptButton = this.btnSubmit;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(464, 375);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSubmit);
        this.Controls.Add(this.txtNotes);
        this.Controls.Add(this.lblNotes);
        this.Controls.Add(this.cmbVisitTime);
        this.Controls.Add(this.lblVisitTime);
        this.Controls.Add(this.dtpVisitDate);
        this.Controls.Add(this.lblVisitDate);
        this.Controls.Add(this.grpPropertyInfo);
        this.Controls.Add(this.pnlHeader);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ScheduleVisitForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Schedule Property Visit";
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        this.grpPropertyInfo.ResumeLayout(false);
        this.grpPropertyInfo.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblHeaderSubtitle;
    private System.Windows.Forms.Label lblHeaderTitle;
    private System.Windows.Forms.GroupBox grpPropertyInfo;
    private System.Windows.Forms.Label lblPropertyLocationVal;
    private System.Windows.Forms.Label lblPropertyLocation;
    private System.Windows.Forms.Label lblPropertyTitleVal;
    private System.Windows.Forms.Label lblPropertyTitle;
    private System.Windows.Forms.Label lblVisitDate;
    private System.Windows.Forms.DateTimePicker dtpVisitDate;
    private System.Windows.Forms.Label lblVisitTime;
    private System.Windows.Forms.ComboBox cmbVisitTime;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Button btnSubmit;
    private System.Windows.Forms.Button btnCancel;
}
