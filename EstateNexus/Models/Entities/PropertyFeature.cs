using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class PropertyFeature
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string Description { get; set; }

        // Navigation Property
        public virtual ICollection<PropertyFeatureMapping> PropertyFeatureMappings { get; set; } = new List<PropertyFeatureMapping>();
    }
}
