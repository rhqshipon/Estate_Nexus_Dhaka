using System;

namespace EstateNexus.Models.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int PropertyId { get; set; }
        public int Rating { get; set; }
        public string ReviewComment { get; set; }
        public string ReviewStatus { get; set; } = "Approved";
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual User Customer { get; set; }
        public virtual Property Property { get; set; }
    }
}
