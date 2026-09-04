using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class PropertyCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
