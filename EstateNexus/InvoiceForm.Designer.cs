namespace EstateNexus
{
    partial class InvoiceForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.lblGeneratedDate = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblCustomerEmail = new System.Windows.Forms.Label();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.lblPaymentStatus = new System.Windows.Forms.Label();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.dgvInvoiceItems = new System.Windows.Forms.DataGridView();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblCommission = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceItems)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(111)))), ((int)(((byte)(168)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "EstateNexus - INVOICE";
            // 
            // pnlInfo
            // 
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.lblInvoiceNo);
            this.pnlInfo.Controls.Add(this.lblGeneratedDate);
            this.pnlInfo.Controls.Add(this.lblCustomerName);
            this.pnlInfo.Controls.Add(this.lblCustomerEmail);
            this.pnlInfo.Controls.Add(this.lblPaymentMethod);
            this.pnlInfo.Controls.Add(this.lblTransactionId);
            this.pnlInfo.Controls.Add(this.lblPaymentStatus);
            this.pnlInfo.Controls.Add(this.lblOrderId);
            this.pnlInfo.Location = new System.Drawing.Point(20, 52);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(720, 115);
            this.pnlInfo.TabIndex = 1;
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInvoiceNo.Location = new System.Drawing.Point(12, 12);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(73, 17);
            this.lblInvoiceNo.TabIndex = 0;
            this.lblInvoiceNo.Text = "Invoice #: ";
            // 
            // lblGeneratedDate
            // 
            this.lblGeneratedDate.AutoSize = true;
            this.lblGeneratedDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGeneratedDate.Location = new System.Drawing.Point(400, 12);
            this.lblGeneratedDate.Name = "lblGeneratedDate";
            this.lblGeneratedDate.Size = new System.Drawing.Size(42, 17);
            this.lblGeneratedDate.TabIndex = 1;
            this.lblGeneratedDate.Text = "Date: ";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCustomerName.Location = new System.Drawing.Point(12, 37);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(71, 17);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "Customer: ";
            // 
            // lblCustomerEmail
            // 
            this.lblCustomerEmail.AutoSize = true;
            this.lblCustomerEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCustomerEmail.Location = new System.Drawing.Point(400, 37);
            this.lblCustomerEmail.Name = "lblCustomerEmail";
            this.lblCustomerEmail.Size = new System.Drawing.Size(46, 17);
            this.lblCustomerEmail.TabIndex = 3;
            this.lblCustomerEmail.Text = "Email: ";
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPaymentMethod.Location = new System.Drawing.Point(12, 62);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(112, 17);
            this.lblPaymentMethod.TabIndex = 4;
            this.lblPaymentMethod.Text = "Payment Method: ";
            // 
            // lblTransactionId
            // 
            this.lblTransactionId.AutoSize = true;
            this.lblTransactionId.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTransactionId.Location = new System.Drawing.Point(400, 62);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(98, 17);
            this.lblTransactionId.TabIndex = 5;
            this.lblTransactionId.Text = "Transaction ID: ";
            // 
            // lblPaymentStatus
            // 
            this.lblPaymentStatus.AutoSize = true;
            this.lblPaymentStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPaymentStatus.Location = new System.Drawing.Point(12, 87);
            this.lblPaymentStatus.Name = "lblPaymentStatus";
            this.lblPaymentStatus.Size = new System.Drawing.Size(103, 17);
            this.lblPaymentStatus.TabIndex = 6;
            this.lblPaymentStatus.Text = "Payment Status: ";
            // 
            // lblOrderId
            // 
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblOrderId.Location = new System.Drawing.Point(400, 87);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(65, 17);
            this.lblOrderId.TabIndex = 7;
            this.lblOrderId.Text = "Order ID: ";
            // 
            // dgvInvoiceItems
            // 
            this.dgvInvoiceItems.AllowUserToAddRows = false;
            this.dgvInvoiceItems.AllowUserToDeleteRows = false;
            this.dgvInvoiceItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoiceItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoiceItems.Location = new System.Drawing.Point(20, 180);
            this.dgvInvoiceItems.MultiSelect = false;
            this.dgvInvoiceItems.Name = "dgvInvoiceItems";
            this.dgvInvoiceItems.ReadOnly = true;
            this.dgvInvoiceItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInvoiceItems.Size = new System.Drawing.Size(720, 220);
            this.dgvInvoiceItems.TabIndex = 2;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubTotal.Location = new System.Drawing.Point(430, 415);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(63, 17);
            this.lblSubTotal.TabIndex = 3;
            this.lblSubTotal.Text = "Subtotal: ";
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDiscount.Location = new System.Drawing.Point(430, 437);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(65, 17);
            this.lblDiscount.TabIndex = 4;
            this.lblDiscount.Text = "Discount: ";
            // 
            // lblCommission
            // 
            this.lblCommission.AutoSize = true;
            this.lblCommission.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCommission.Location = new System.Drawing.Point(430, 459);
            this.lblCommission.Name = "lblCommission";
            this.lblCommission.Size = new System.Drawing.Size(163, 17);
            this.lblCommission.TabIndex = 5;
            this.lblCommission.Text = "Platform Commission (5%): ";
            // 
            // lblTotalPaid
            // 
            this.lblTotalPaid.AutoSize = true;
            this.lblTotalPaid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPaid.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTotalPaid.Location = new System.Drawing.Point(430, 485);
            this.lblTotalPaid.Name = "lblTotalPaid";
            this.lblTotalPaid.Size = new System.Drawing.Size(94, 21);
            this.lblTotalPaid.TabIndex = 6;
            this.lblTotalPaid.Text = "Total Paid: ";
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Cambria", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(480, 540);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(120, 36);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Cambria", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(620, 540);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 36);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 595);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.lblTotalPaid);
            this.Controls.Add(this.lblCommission);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.lblSubTotal);
            this.Controls.Add(this.dgvInvoiceItems);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Invoice Details - EstateNexus";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceItems)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Label lblGeneratedDate;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerEmail;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.Label lblPaymentStatus;
        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.DataGridView dgvInvoiceItems;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblCommission;
        private System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}
