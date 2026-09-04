using System;

namespace EstateNexus.Models.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; } = "Completed";
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
