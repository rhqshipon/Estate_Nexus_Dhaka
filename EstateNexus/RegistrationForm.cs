using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            cmbRole.SelectedIndex = 0; // Default to Customer
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString() ?? "Customer";

            // 1. Check required fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in Full Name, Username, Email, and Password.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validate Username
            if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (username.Contains(" "))
            {
                MessageBox.Show("Username cannot contain spaces.", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            // 3. Validate Email format (check for '@' symbol and valid domain format)
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address (e.g., name@example.com).", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    // 4. Check if Email already exists
                    if (context.Users.Any(u => u.Email == email))
                    {
                        MessageBox.Show("Email is already registered. Please login or use a different email.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }

                    // 5. Insert new user with RoleId and Hashed Password
                    var roleObj = context.Roles.FirstOrDefault(r => r.RoleName == role)
                                  ?? context.Roles.FirstOrDefault(r => r.RoleName == "Customer");

                    string hashedPassword = PasswordHelper.HashPassword(password);
                    var newUser = new User
                    {
                        RoleId = roleObj.RoleId,
                        FullName = name,
                        Email = email,
                        Phone = phone,
                        PasswordHash = hashedPassword,
                        Address = "",
                        ProfileImagePath = null,
                        AccountStatus = "Active",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    MessageBox.Show("Registration Successful! Please login with your email address.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Basic check: must contain '@' and '.' after '@', and not start or end with '@' or '.'
            int atIndex = email.IndexOf('@');
            if (atIndex <= 0 || atIndex == email.Length - 1)
                return false;

            if (email.IndexOf('@', atIndex + 1) != -1) // only one '@' allowed
                return false;

            string domain = email.Substring(atIndex + 1);
            if (!domain.Contains(".") || domain.StartsWith(".") || domain.EndsWith("."))
                return false;

            // Regex pattern check for standard email formatting
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
