using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class InvoiceForm : Form
    {
        private readonly int _orderId;

        // Cached print details
        private string _invoiceNumber = "";
        private string _generatedDate = "";
        private string _customerName = "";
        private string _customerEmail = "";
        private string _paymentMethod = "";
        private string _transactionId = "";
        private string _paymentStatus = "";
        private string _subtotalText = "";
        private string _discountText = "";
        private string _commissionText = "";
        private string _totalPaidText = "";

        private class InvoicePrintItem
        {
            public string PropertyTitle { get; set; }
            public string OwnerName { get; set; }
            public string ListingType { get; set; }
            public string RentalMonths { get; set; }
            public string UnitPrice { get; set; }
            public string FinalAmount { get; set; }
        }

        private readonly List<InvoicePrintItem> _printItems = new List<InvoicePrintItem>();

        public InvoiceForm(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            LoadInvoiceData();
        }

        private void LoadInvoiceData()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var order = context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Payments)
                    .Include(o => o.Invoice)
                    .Include(o => o.Commission)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Property)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Owner)
                    .FirstOrDefault(o => o.OrderId == _orderId);

                if (order == null)
                {
                    MessageBox.Show("Order not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Security check: If current user is Customer, ensure they own this order
                if (Session.Role != null && Session.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase) && order.CustomerId != Session.UserId)
                {
                    MessageBox.Show("Access denied. You cannot view another customer's invoice.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                var payment = order.Payments?.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
                var invoice = order.Invoice;
                var commission = order.Commission;

                _invoiceNumber = invoice != null ? invoice.InvoiceNumber : $"INV-{order.OrderDate:yyyyMMdd}-{order.OrderId}";
                _generatedDate = invoice != null ? invoice.GeneratedDate.ToString("yyyy-MM-dd HH:mm") : order.OrderDate.ToString("yyyy-MM-dd HH:mm");
                _customerName = order.Customer != null ? order.Customer.FullName : "Customer #" + order.CustomerId;
                _customerEmail = order.Customer != null ? order.Customer.Email : "N/A";
                _paymentMethod = payment != null ? payment.PaymentMethod : "Online/Card";
                _transactionId = payment != null ? payment.TransactionId : "N/A";
                _paymentStatus = payment != null ? payment.PaymentStatus : order.OrderStatus;

                decimal subtotal = invoice?.SubTotal ?? order.TotalAmount;
                decimal discount = invoice?.DiscountAmount ?? 0m;
                decimal commAmount = invoice?.CommissionAmount ?? (commission?.CommissionAmount ?? Math.Round(order.TotalAmount * 0.05m, 2));
                decimal total = invoice?.TotalAmount ?? order.TotalAmount;

                _subtotalText = "৳" + subtotal.ToString("N2");
                _discountText = "৳" + discount.ToString("N2");
                _commissionText = "৳" + commAmount.ToString("N2");
                _totalPaidText = "৳" + total.ToString("N2");

                // Update UI Labels
                lblInvoiceNo.Text = "Invoice #: " + _invoiceNumber;
                lblGeneratedDate.Text = "Date: " + _generatedDate;
                lblCustomerName.Text = "Customer: " + _customerName;
                lblCustomerEmail.Text = "Email: " + _customerEmail;
                lblPaymentMethod.Text = "Payment Method: " + _paymentMethod;
                lblTransactionId.Text = "Transaction ID: " + _transactionId;
                lblPaymentStatus.Text = "Payment Status: " + _paymentStatus;
                lblOrderId.Text = "Order ID: #" + order.OrderId;

                lblSubTotal.Text = "Subtotal: " + _subtotalText;
                lblDiscount.Text = "Discount: " + _discountText;
                lblCommission.Text = "Platform Commission (5%): " + _commissionText;
                lblTotalPaid.Text = "Total Paid: " + _totalPaidText;

                // Bind Items
                _printItems.Clear();
                var itemsDisplay = new List<object>();

                if (order.OrderItems != null)
                {
                    foreach (var oi in order.OrderItems)
                    {
                        string propTitle = oi.Property != null ? oi.Property.PropertyTitle : "Property #" + oi.PropertyId;
                        string ownerName = oi.Owner != null ? oi.Owner.FullName : "Owner #" + oi.OwnerId;
                        string listType = oi.Property != null ? oi.Property.ListingType : "N/A";
                        string rentalMos = oi.RentalMonths > 0 ? oi.RentalMonths.ToString() : "-";
                        string unitP = "৳" + oi.UnitPrice.ToString("N2");
                        string finalAmt = "৳" + oi.FinalAmount.ToString("N2");

                        _printItems.Add(new InvoicePrintItem
                        {
                            PropertyTitle = propTitle,
                            OwnerName = ownerName,
                            ListingType = listType,
                            RentalMonths = rentalMos,
                            UnitPrice = unitP,
                            FinalAmount = finalAmt
                        });

                        itemsDisplay.Add(new
                        {
                            PropertyTitle = propTitle,
                            Owner = ownerName,
                            ListingType = listType,
                            RentalMonths = rentalMos,
                            UnitPrice = unitP,
                            FinalAmount = finalAmt
                        });
                    }
                }

                dgvInvoiceItems.DataSource = itemsDisplay;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading invoice: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening print preview: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            using var titleFont = new Font("Cambria", 18, FontStyle.Bold);
            using var headerFont = new Font("Segoe UI", 11, FontStyle.Bold);
            using var regularFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using var boldFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var smallFont = new Font("Segoe UI", 8.5f, FontStyle.Regular);

            float startX = 50;
            float y = 50;
            float tableWidth = 700;
            float lineHeight = 22;

            // Brand Header
            g.DrawString("EstateNexus - INVOICE", titleFont, Brushes.DarkSlateBlue, startX, y);
            y += 35;
            g.DrawLine(Pens.Gray, startX, y, startX + tableWidth, y);
            y += 15;

            // Metadata section
            g.DrawString($"Invoice #: {_invoiceNumber}", boldFont, Brushes.Black, startX, y);
            g.DrawString($"Date: {_generatedDate}", regularFont, Brushes.Black, startX + 380, y);
            y += lineHeight;

            g.DrawString($"Customer: {_customerName}", regularFont, Brushes.Black, startX, y);
            g.DrawString($"Email: {_customerEmail}", regularFont, Brushes.Black, startX + 380, y);
            y += lineHeight;

            g.DrawString($"Payment Method: {_paymentMethod}", regularFont, Brushes.Black, startX, y);
            g.DrawString($"Transaction ID: {_transactionId}", regularFont, Brushes.Black, startX + 380, y);
            y += lineHeight;

            g.DrawString($"Payment Status: {_paymentStatus}", regularFont, Brushes.Black, startX, y);
            g.DrawString($"Order ID: #{_orderId}", regularFont, Brushes.Black, startX + 380, y);
            y += lineHeight + 15;

            // Order items table header
            g.DrawLine(Pens.Black, startX, y, startX + tableWidth, y);
            y += 5;
            g.DrawString("Property Title", headerFont, Brushes.Black, startX, y);
            g.DrawString("Type", headerFont, Brushes.Black, startX + 240, y);
            g.DrawString("Months", headerFont, Brushes.Black, startX + 320, y);
            g.DrawString("Unit Price", headerFont, Brushes.Black, startX + 410, y);
            g.DrawString("Final Amount", headerFont, Brushes.Black, startX + 560, y);
            y += 24;
            g.DrawLine(Pens.Gray, startX, y, startX + tableWidth, y);
            y += 10;

            // Table rows
            foreach (var item in _printItems)
            {
                g.DrawString(item.PropertyTitle, regularFont, Brushes.Black, startX, y);
                g.DrawString(item.ListingType, regularFont, Brushes.Black, startX + 240, y);
                g.DrawString(item.RentalMonths, regularFont, Brushes.Black, startX + 320, y);
                g.DrawString(item.UnitPrice, regularFont, Brushes.Black, startX + 410, y);
                g.DrawString(item.FinalAmount, regularFont, Brushes.Black, startX + 560, y);
                y += lineHeight;
            }

            y += 15;
            g.DrawLine(Pens.Gray, startX, y, startX + tableWidth, y);
            y += 15;

            // Summary Totals
            g.DrawString($"Subtotal: {_subtotalText}", boldFont, Brushes.Black, startX + 410, y);
            y += lineHeight;
            g.DrawString($"Discount: {_discountText}", regularFont, Brushes.Black, startX + 410, y);
            y += lineHeight;
            g.DrawString($"Platform Commission (5%): {_commissionText}", regularFont, Brushes.DarkSlateGray, startX + 410, y);
            y += lineHeight;
            g.DrawLine(Pens.Black, startX + 410, y, startX + tableWidth, y);
            y += 6;
            g.DrawString($"Total Paid: {_totalPaidText}", boldFont, Brushes.DarkGreen, startX + 410, y);
            y += 40;

            g.DrawString("Thank you for using EstateNexus! For queries, contact support@estatenexus.com", smallFont, Brushes.Gray, startX, y);

            e.HasMorePages = false;
        }
    }
}
