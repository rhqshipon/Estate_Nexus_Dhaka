using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

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
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "SELECT CategoryId, CategoryName FROM PropertyCategories";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cmbCategory.DisplayMember = "CategoryName";
                        cmbCategory.ValueMember = "CategoryId";
                        cmbCategory.DataSource = dt;
                    }
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
            int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = @"
                        INSERT INTO Properties (OwnerId, CategoryId, PropertyName, ListingType, Location, Address, Area, Bedrooms, Bathrooms, Price, Description, Status)
                        VALUES (@OwnerId, @CategoryId, @PropertyName, @ListingType, @Location, @Address, @Area, @Bedrooms, @Bathrooms, @Price, @Description, 'Available')";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OwnerId", Session.UserId);
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.Parameters.AddWithValue("@PropertyName", name);
                        cmd.Parameters.AddWithValue("@ListingType", listingType);
                        cmd.Parameters.AddWithValue("@Location", location);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Area", area);
                        cmd.Parameters.AddWithValue("@Bedrooms", bedrooms);
                        cmd.Parameters.AddWithValue("@Bathrooms", bathrooms);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Description", desc);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Property added successfully!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
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
