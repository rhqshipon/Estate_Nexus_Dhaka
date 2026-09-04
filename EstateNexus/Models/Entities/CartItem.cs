using System;

namespace EstateNexus.Models.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int PropertyId { get; set; }
        public int RentalMonths { get; set; } = 1;
        public decimal? OfferedPrice { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Cart Cart { get; set; }
        public virtual Property Property { get; set; }
    }
}
