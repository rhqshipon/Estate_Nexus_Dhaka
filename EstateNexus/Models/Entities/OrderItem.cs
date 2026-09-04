namespace EstateNexus.Models.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int PropertyId { get; set; }
        public int OwnerId { get; set; }
        public int Quantity { get; set; } = 1;
        public int RentalMonths { get; set; } = 0;
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal FinalAmount { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Property Property { get; set; }
        public virtual User Owner { get; set; }
    }
}
