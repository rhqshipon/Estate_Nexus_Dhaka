using System;

namespace EstateNexus.Models.Entities
{
    public class Commission
    {
        public int CommissionId { get; set; }
        public int OrderId { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal OwnerAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Order Order { get; set; }
    }
}
