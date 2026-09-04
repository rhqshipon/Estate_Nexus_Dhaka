using System;

namespace EstateNexus.Models.Entities
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int PropertyId { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Property Property { get; set; }
    }
}
