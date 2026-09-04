using System;

namespace EstateNexus.Models.Entities
{
    public class VisitRequest
    {
        public int VisitRequestId { get; set; }
        public int CustomerId { get; set; }
        public int PropertyId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string RequestStatus { get; set; } = "Pending";
        public string CustomerNote { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual User Customer { get; set; }
        public virtual Property Property { get; set; }
    }
}
