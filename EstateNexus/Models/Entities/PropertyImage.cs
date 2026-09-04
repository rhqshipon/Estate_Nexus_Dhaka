using System;

namespace EstateNexus.Models.Entities
{
    public class PropertyImage
    {
        public int ImageId { get; set; }
        public int PropertyId { get; set; }
        public string ImagePath { get; set; }
        public bool IsPrimary { get; set; } = false;
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Property Property { get; set; }
    }
}
