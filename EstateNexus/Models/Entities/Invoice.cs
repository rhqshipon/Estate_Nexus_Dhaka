using System;

namespace EstateNexus.Models.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal CommissionAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
