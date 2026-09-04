using System;
using System.Linq;
using System.Windows.Forms;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class AddPropertyForm : Form
    {
        public AddPropertyForm()
        {
            InitializeComponent();
        }

        private void AddPropertyForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            cmbListingType.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var categories = context.PropertyCategories
                        .Where(c => c.IsActive)
                        .Select(c => new { c.CategoryId, c.CategoryName })
                        .ToList();

                    cmbCategory.DisplayMember = "CategoryName";
                    cmbCategory.ValueMember = "CategoryId";
                    cmbCategory.DataSource = categories;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtPropertyName.Text.Trim();
            string location = txtLocation.Text.Trim();
            string address = txtAddress.Text.Trim();
            string listingType = cmbListingType.SelectedItem?.ToString() ?? "Sale";
            string priceStr = txtPrice.Text.Trim();
            string desc = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(priceStr))
            {
                MessageBox.Show("Please fill in Title, Location, and Price.");
                return;
            }

            if (!decimal.TryParse(priceStr, out decimal price))
            {
                MessageBox.Show("Please enter a valid numeric price.");
                return;
            }

            int.TryParse(txtArea.Text.Trim(), out int area);
            int bedrooms = (int)numBedrooms.Value;
            int bathrooms = (int)numBathrooms.Value;
            if (cmbCategory.SelectedValue == null || !int.TryParse(cmbCategory.SelectedValue.ToString(), out int categoryId) || categoryId <= 0)
            {
                MessageBox.Show("Please select a valid property category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ownerId = Session.UserId;
            if (ownerId <= 0)
            {
                using var contextCheck = new EstateNexusDbContext();
                var defaultSeller = contextCheck.Users.FirstOrDefault(u => u.RoleId == 2);
                if (defaultSeller != null)
                    ownerId = defaultSeller.UserId;
            }

            string district = "Dhaka";
            string areaLocation = location;
            if (location.Contains(","))
            {
                var parts = location.Split(',');
                areaLocation = parts[0].Trim();
                if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
                    district = parts[1].Trim();
            }

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    var property = new Property
                    {
                        OwnerId = ownerId,
                        CategoryId = categoryId,
                        PropertyTitle = name,
                        ListingType = listingType,
                        District = district,
                        AreaLocation = areaLocation,
                        FullAddress = string.IsNullOrEmpty(address) ? location : address,
                        AreaSize = area > 0 ? (decimal)area : 1000m,
                        AreaUnit = "sqft",
                        Bedrooms = bedrooms,
                        Bathrooms = bathrooms,
                        Price = price,
                        Description = desc,
                        PropertyStatus = "Available",
                        ApprovalStatus = "Approved",
                        IsFeatured = false,
                        CreatedDate = DateTime.Now
                    };

                    context.Properties.Add(property);
                    context.SaveChanges();

                    MessageBox.Show("Property added successfully!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving property: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
