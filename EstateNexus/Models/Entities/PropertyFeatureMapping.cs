namespace EstateNexus.Models.Entities
{
    public class PropertyFeatureMapping
    {
        public int PropertyId { get; set; }
        public int FeatureId { get; set; }

        // Navigation Properties
        public virtual Property Property { get; set; }
        public virtual PropertyFeature Feature { get; set; }
    }
}
