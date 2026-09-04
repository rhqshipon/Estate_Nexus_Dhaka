using System;
using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual User Customer { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
