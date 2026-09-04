using System;

namespace EstateNexus.Models.Entities
{
    public class FeaturedListing
    {
        public int FeaturedListingId { get; set; }
        public int PropertyId { get; set; }
        public decimal FeaturedFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string Status { get; set; } = "Active";
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Property Property { get; set; }
    }
}
