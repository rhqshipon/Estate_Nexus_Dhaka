using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class AddPropertyForm : Form
    {
        private readonly int _propertyId = 0;
        private string _selectedImageSourcePath = null;

        public AddPropertyForm()
        {
            InitializeComponent();
            _propertyId = 0;
        }

        public AddPropertyForm(int propertyId)
        {
            InitializeComponent();
            _propertyId = propertyId;
        }

        private void AddPropertyForm_Load(object sender, EventArgs e)
        {
            LoadCategories();

            if (_propertyId == 0)
            {
                cmbListingType.SelectedIndex = 0;
            }
            else
            {
                LoadPropertyForEdit(_propertyId);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var categories = context.PropertyCategories
                    .Where(c => c.IsActive)
                    .Select(c => new { c.CategoryId, c.CategoryName })
                    .ToList();

                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void LoadPropertyForEdit(int propertyId)
        {
            try
            {
                using var context = new EstateNexusDbContext();
                var prop = context.Properties
                    .Include(p => p.PropertyImages)
                    .FirstOrDefault(p => p.PropertyId == propertyId);

                if (prop == null || (Session.UserId > 0 && prop.OwnerId != Session.UserId))
                {
                    MessageBox.Show("Access denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                this.Text = "Edit Property - EstateNexus";
                lblTitle.Text = "Edit Property";
                btnSave.Text = "Update Property";

                txtPropertyName.Text = prop.PropertyTitle;
                cmbCategory.SelectedValue = prop.CategoryId;
                cmbListingType.SelectedItem = prop.ListingType;
                txtLocation.Text = !string.IsNullOrEmpty(prop.District) && prop.District != "Dhaka"
                    ? $"{prop.AreaLocation}, {prop.District}"
                    : prop.AreaLocation;
                txtAddress.Text = prop.FullAddress;
                txtArea.Text = prop.AreaSize.ToString("0.##");
                numBedrooms.Value = Math.Max(numBedrooms.Minimum, Math.Min(numBedrooms.Maximum, prop.Bedrooms));
                numBathrooms.Value = Math.Max(numBathrooms.Minimum, Math.Min(numBathrooms.Maximum, prop.Bathrooms));
                txtPrice.Text = prop.Price.ToString("0.##");
                txtDescription.Text = prop.Description ?? "";

                var img = prop.PropertyImages.FirstOrDefault(pi => pi.IsPrimary) ?? prop.PropertyImages.FirstOrDefault();
                if (img != null && !string.IsNullOrEmpty(img.ImagePath))
                {
                    string fullPath = Path.IsPathRooted(img.ImagePath)
                        ? img.ImagePath
                        : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, img.ImagePath);

                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                            picImagePreview.Image?.Dispose();
                            picImagePreview.Image = Image.FromStream(stream);
                            lblImagePath.Text = Path.GetFileName(img.ImagePath);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading property for edit: " + ex.Message);
            }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select Property Image"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _selectedImageSourcePath = ofd.FileName;
                    lblImagePath.Text = Path.GetFileName(_selectedImageSourcePath);

                    using var stream = new FileStream(_selectedImageSourcePath, FileMode.Open, FileAccess.Read);
                    picImagePreview.Image?.Dispose();
                    picImagePreview.Image = Image.FromStream(stream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error previewing image: " + ex.Message);
                }
            }
        }

        private string SavePropertyImageFile(string sourcePath, int propertyId)
        {
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                return null;

            try
            {
                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PropertyImages");
                if (!Directory.Exists(imagesDir))
                {
                    Directory.CreateDirectory(imagesDir);
                }

                string ext = Path.GetExtension(sourcePath);
                string fileName = $"prop_{propertyId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                string destPath = Path.Combine(imagesDir, fileName);
                File.Copy(sourcePath, destPath, true);
                return Path.Combine("PropertyImages", fileName);
            }
            catch
            {
                return null;
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

            // Validation
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Property Title is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPropertyName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(location))
            {
                MessageBox.Show("Location is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedValue == null || !int.TryParse(cmbCategory.SelectedValue.ToString(), out int categoryId) || categoryId <= 0)
            {
                MessageBox.Show("Please select a valid property category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }

            if (!decimal.TryParse(priceStr, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (!decimal.TryParse(txtArea.Text.Trim(), out decimal area) || area <= 0)
            {
                MessageBox.Show("Please enter a valid area size greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtArea.Focus();
                return;
            }

            int bedrooms = (int)numBedrooms.Value;
            int bathrooms = (int)numBathrooms.Value;
            if (bedrooms < 0 || bathrooms < 0)
            {
                MessageBox.Show("Bedrooms and bathrooms must be 0 or greater.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                using var context = new EstateNexusDbContext();

                if (_propertyId == 0)
                {
                    // ADD MODE
                    int ownerId = Session.UserId;
                    if (ownerId <= 0)
                    {
                        var defaultSeller = context.Users.FirstOrDefault(u => u.RoleId == 2);
                        if (defaultSeller != null)
                            ownerId = defaultSeller.UserId;
                    }

                    var property = new Property
                    {
                        OwnerId = ownerId,
                        CategoryId = categoryId,
                        PropertyTitle = name,
                        ListingType = listingType,
                        District = district,
                        AreaLocation = areaLocation,
                        FullAddress = string.IsNullOrEmpty(address) ? location : address,
                        AreaSize = area,
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

                    if (!string.IsNullOrEmpty(_selectedImageSourcePath))
                    {
                        string relPath = SavePropertyImageFile(_selectedImageSourcePath, property.PropertyId);
                        if (relPath != null)
                        {
                            var propImg = new PropertyImage
                            {
                                PropertyId = property.PropertyId,
                                ImagePath = relPath,
                                IsPrimary = true,
                                UploadedDate = DateTime.Now
                            };
                            context.PropertyImages.Add(propImg);
                            context.SaveChanges();
                        }
                    }

                    MessageBox.Show("Property added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // EDIT MODE
                    var property = context.Properties
                        .Include(p => p.PropertyImages)
                        .FirstOrDefault(p => p.PropertyId == _propertyId);

                    if (property == null || (Session.UserId > 0 && property.OwnerId != Session.UserId))
                    {
                        MessageBox.Show("Access denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    property.PropertyTitle = name;
                    property.CategoryId = categoryId;
                    property.ListingType = listingType;
                    property.District = district;
                    property.AreaLocation = areaLocation;
                    property.FullAddress = string.IsNullOrEmpty(address) ? location : address;
                    property.AreaSize = area;
                    property.Bedrooms = bedrooms;
                    property.Bathrooms = bathrooms;
                    property.Price = price;
                    property.Description = desc;
                    property.UpdatedDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(_selectedImageSourcePath))
                    {
                        string relPath = SavePropertyImageFile(_selectedImageSourcePath, property.PropertyId);
                        if (relPath != null)
                        {
                            var existingImg = property.PropertyImages.FirstOrDefault(pi => pi.IsPrimary) ?? property.PropertyImages.FirstOrDefault();
                            if (existingImg != null)
                            {
                                existingImg.ImagePath = relPath;
                                existingImg.UploadedDate = DateTime.Now;
                            }
                            else
                            {
                                var propImg = new PropertyImage
                                {
                                    PropertyId = property.PropertyId,
                                    ImagePath = relPath,
                                    IsPrimary = true,
                                    UploadedDate = DateTime.Now
                                };
                                context.PropertyImages.Add(propImg);
                            }
                        }
                    }

                    context.SaveChanges();
                    MessageBox.Show("Property updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
