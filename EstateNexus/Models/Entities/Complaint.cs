using System;

namespace EstateNexus.Models.Entities
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public int CustomerId { get; set; }
        public int? PropertyId { get; set; }
        public string Subject { get; set; }
        public string ComplaintType { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; } = "Normal";
        public string ComplaintStatus { get; set; } = "Pending";
        public int? ResolvedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ResolvedDate { get; set; }

        // Navigation Properties
        public virtual User Customer { get; set; }
        public virtual Property Property { get; set; }
        public virtual User Resolver { get; set; }
    }
}
