using System;
using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class Property
    {
        public int PropertyId { get; set; }
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public string PropertyTitle { get; set; }
        public string ListingType { get; set; }
        public string District { get; set; }
        public string AreaLocation { get; set; }
        public string FullAddress { get; set; }
        public decimal AreaSize { get; set; }
        public string AreaUnit { get; set; } = "sqft";
        public int Bedrooms { get; set; } = 0;
        public int Bathrooms { get; set; } = 0;
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string PropertyStatus { get; set; } = "Available";
        public string ApprovalStatus { get; set; } = "Pending";
        public bool IsFeatured { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        public virtual User Owner { get; set; }
        public virtual PropertyCategory Category { get; set; }
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<PropertyFeatureMapping> PropertyFeatureMappings { get; set; } = new List<PropertyFeatureMapping>();
        public virtual ICollection<FeaturedListing> FeaturedListings { get; set; } = new List<FeaturedListing>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<VisitRequest> VisitRequests { get; set; } = new List<VisitRequest>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
    }
}
