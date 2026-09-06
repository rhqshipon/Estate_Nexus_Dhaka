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
            // Clear previous errors
            errRegistration.Clear();
            lblRegError.Text = string.Empty;

            // Get values from the form
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            string role = cmbRole.SelectedItem?.ToString()?.Trim() ?? "Customer";

            // ============================================================
            // 1. REQUIRED FIELD VALIDATION
            // ============================================================
            if (string.IsNullOrWhiteSpace(name))
            {
                errRegistration.SetError(txtName, "Full Name is required.");
                lblRegError.Text = "Please enter your Full Name.";
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errRegistration.SetError(txtEmail, "Email is required.");
                lblRegError.Text = "Please enter your Email address.";
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                errRegistration.SetError(txtPassword, "Password is required.");
                lblRegError.Text = "Please enter your Password.";
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                errRegistration.SetError(txtConfirmPassword, "Please confirm your password.");
                lblRegError.Text = "Please confirm your password.";
                txtConfirmPassword.Focus();
                return;
            }

            // ============================================================
            // 2. EMAIL VALIDATION
            // ============================================================
            if (!IsValidEmail(email))
            {
                errRegistration.SetError(txtEmail, "Invalid email address format.");
                lblRegError.Text = "Please enter a valid email address.";
                txtEmail.Focus();
                return;
            }

            // ============================================================
            // 3. PHONE VALIDATION (If provided)
            // ============================================================
            if (!string.IsNullOrWhiteSpace(phone) && !Regex.IsMatch(phone, @"^[0-9+\-\s()]{7,20}$"))
            {
                errRegistration.SetError(txtPhone, "Please enter a valid phone number.");
                lblRegError.Text = "Please enter a valid phone number.";
                txtPhone.Focus();
                return;
            }

            // ============================================================
            // 4. PASSWORD STRENGTH VALIDATION
            // ============================================================
            if (password.Length < 6)
            {
                errRegistration.SetError(txtPassword, "Password must be at least 6 characters.");
                lblRegError.Text = "Password must be at least 6 characters long.";
                txtPassword.Focus();
                return;
            }

            // ============================================================
            // 5. CONFIRM PASSWORD MATCHING
            // ============================================================
            if (password != confirmPassword)
            {
                errRegistration.SetError(txtConfirmPassword, "Passwords do not match.");
                lblRegError.Text = "Passwords do not match.";
                txtConfirmPassword.Focus();
                return;
            }

            // ============================================================
            // 6. DATABASE REGISTRATION
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
                        errRegistration.SetError(txtEmail, "This email is already registered.");
                        lblRegError.Text = "This email is already registered. Please use a different email.";
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


                        AccountStatus = (roleObj.RoleName == "Admin") ? "Pending" : "Active",

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

                    if (roleObj.RoleName == "Admin")
                    {
                        MessageBox.Show(
                            "Registration Successful!\n\nYour seller account is pending Super Admin approval.",
                            "Registration Pending",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Registration Successful!\n\nYou can now login using your email address and password.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }


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