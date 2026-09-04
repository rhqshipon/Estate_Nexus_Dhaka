using System;
using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Completed";
        public string TransactionType { get; set; } = "Sale";

        // Navigation Properties
        public virtual User Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual Invoice Invoice { get; set; }
        public virtual Commission Commission { get; set; }
    }
}
