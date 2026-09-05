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

            // Default role
            cmbRole.SelectedIndex = 0;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Get values from the form
            string name = txtName.Text.Trim();

            // Username is intentionally ignored.
            // EstateNexus database does not have a Username column.
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text.Trim();

            string role = cmbRole.SelectedItem?.ToString()?.Trim() ?? "Customer";


            // ============================================================
            // 1. REQUIRED FIELD VALIDATION
            // ============================================================

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please fill in Full Name, Email, and Password.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            // ============================================================
            // 2. EMAIL VALIDATION
            // ============================================================

            if (!IsValidEmail(email))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtEmail.Focus();
                return;
            }


            // ============================================================
            // 3. DATABASE REGISTRATION
            // ============================================================

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    // ----------------------------------------------------
                    // Check if email already exists
                    // ----------------------------------------------------

                    bool emailExists = context.Users
                        .Any(u => u.Email == email);

                    if (emailExists)
                    {
                        MessageBox.Show(
                            "This email is already registered. Please use a different email.",
                            "Duplicate Email",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        txtEmail.Focus();
                        return;
                    }


                    // ----------------------------------------------------
                    // Find selected role
                    // ----------------------------------------------------

                    var roleObj = context.Roles
                        .FirstOrDefault(r => r.RoleName == role);


                    // If selected role is not found,
                    // fallback to Customer
                    if (roleObj == null)
                    {
                        roleObj = context.Roles
                            .FirstOrDefault(r => r.RoleName == "Customer");
                    }


                    // Stop if no valid role exists
                    if (roleObj == null)
                    {
                        MessageBox.Show(
                            "No valid role was found in the database.",
                            "Role Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        return;
                    }


                    // ----------------------------------------------------
                    // Hash Password
                    // ----------------------------------------------------

                    string hashedPassword =
                        PasswordHelper.HashPassword(password);


                    // ----------------------------------------------------
                    // Create New User
                    // ----------------------------------------------------

                    var newUser = new User
                    {
                        RoleId = roleObj.RoleId,

                        FullName = name,

                        Email = email,

                        Phone = phone,

                        PasswordHash = hashedPassword,

                        Address = string.Empty,

                        ProfileImagePath = null,


                        // IMPORTANT:
                        // Your SQL Server CHECK constraint does NOT allow
                        // "Active".
                        //
                        // It currently allows "Pending" / "Suspended".
                        //
                        AccountStatus = "Pending",


                        IsActive = true,

                        CreatedDate = DateTime.Now
                    };


                    // ----------------------------------------------------
                    // Save User
                    // ----------------------------------------------------

                    context.Users.Add(newUser);

                    context.SaveChanges();


                    // ----------------------------------------------------
                    // Success Message
                    // ----------------------------------------------------

                    MessageBox.Show(
                        "Registration Successful!\n\nYou can now login using your email address and password.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );


                    // Go back to Login Form
                    LoginForm loginForm = new LoginForm();

                    loginForm.Show();

                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                // Show detailed database error
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage +=
                        "\n\nInner Error:\n" +
                        ex.InnerException.Message;
                }

                MessageBox.Show(
                    "Registration Error!\n\n" + errorMessage,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // ================================================================
        // EMAIL VALIDATION METHOD
        // ================================================================

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            int atIndex = email.IndexOf('@');

            if (atIndex <= 0 || atIndex == email.Length - 1)
            {
                return false;
            }


            // Only one @ is allowed
            if (email.IndexOf('@', atIndex + 1) != -1)
            {
                return false;
            }


            string domain = email.Substring(atIndex + 1);

            if (!domain.Contains(".") ||
                domain.StartsWith(".") ||
                domain.EndsWith("."))
            {
                return false;
            }


            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }


        // ================================================================
        // CANCEL BUTTON
        // ================================================================

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();

            loginForm.Show();

            this.Hide();
        }


        // ================================================================
        // CLOSE APPLICATION
        // ================================================================

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Environment.Exit(0);
        }
    }
}