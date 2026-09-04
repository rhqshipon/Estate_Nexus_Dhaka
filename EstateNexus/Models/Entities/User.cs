using System;
using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Address { get; set; }
        public string ProfileImagePath { get; set; }
        public string AccountStatus { get; set; } = "Active";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Role Role { get; set; }
        public virtual ICollection<Property> OwnedProperties { get; set; } = new List<Property>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<OrderItem> OwnedOrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<VisitRequest> VisitRequests { get; set; } = new List<VisitRequest>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Complaint> SubmittedComplaints { get; set; } = new List<Complaint>();
        public virtual ICollection<Complaint> ResolvedComplaints { get; set; } = new List<Complaint>();
    }
}
