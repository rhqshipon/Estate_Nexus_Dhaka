using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using EstateNexus.Data;
using EstateNexus.Models.Entities;

namespace EstateNexus
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string input = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Clear previous validation errors
            errLogin.Clear();
            lblLoginError.Text = string.Empty;

            bool isUsernameEmpty = string.IsNullOrEmpty(input);
            bool isPasswordEmpty = string.IsNullOrEmpty(password);

            // Validate empty fields with inline indicators
            if (isUsernameEmpty && isPasswordEmpty)
            {
                errLogin.SetError(txtUsername, "Email or username is required.");
                errLogin.SetError(txtPassword, "Password is required.");
                lblLoginError.Text = "Please enter your email or username and password.";
                return;
            }

            if (isUsernameEmpty)
            {
                errLogin.SetError(txtUsername, "Email or username is required.");
                lblLoginError.Text = "Please enter your email or username.";
                return;
            }

            if (isPasswordEmpty)
            {
                errLogin.SetError(txtPassword, "Password is required.");
                lblLoginError.Text = "Please enter your password.";
                return;
            }

            try
            {
                using (var context = new EstateNexusDbContext())
                {
                    string targetEmail = input.Contains("@") ? input : input + "@estatenexus.com";
                    var user = context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Email == input || u.Email == targetEmail);

                    if (user != null)
                    {
                        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
                        {
                            MessageBox.Show("Invalid email/username or password.");
                            return;
                        }

                        // Normalize status
                        string status = (user.AccountStatus ?? "").Trim();

                        if (string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase))
                        {
                            status = "Active";
                        }
                        else if (string.Equals(status, "Rejected", StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(status, "Inactive", StringComparison.OrdinalIgnoreCase))
                        {
                            status = "Suspended";
                        }

                        if (string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Your account is awaiting Super Admin approval.");
                            return;
                        }

                        if (string.Equals(status, "Suspended", StringComparison.OrdinalIgnoreCase) || !user.IsActive)
                        {
                            MessageBox.Show("Your account has been suspended. Contact EstateNexus support.");
                            return;
                        }

                        if (!string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Account is not active.");
                            return;
                        }

                        Session.UserId = user.UserId;
                        Session.FullName = user.FullName;
                        Session.Email = user.Email;
                        Session.Username = user.Email;
                        Session.Role = user.Role?.RoleName ?? "Customer";
                        Session.ProfileImagePath = user.ProfileImagePath;

                        // If stored password was plain text, upgrade to SHA-256
                        if (!PasswordHelper.IsHashed(user.PasswordHash))
                        {
                            user.PasswordHash = PasswordHelper.HashPassword(password);
                            context.SaveChanges();
                        }

                        MessageBox.Show("Login Successful! Welcome " + Session.FullName);
                        this.Hide();

                        if (Session.Role == "SuperAdmin")
                        {
                            new SuperAdminDashboard().Show();
                        }
                        else if (Session.Role == "Admin")
                        {
                            new AdminDashboard().Show();
                        }
                        else
                        {
                            new CustomerDashboard().Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid email/username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm();
            regForm.Show();
            this.Hide();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle between hidden ('*') and visible ('\0') password characters
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            chkShowPassword.Checked = false;
            errLogin.Clear();
            lblLoginError.Text = string.Empty;
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // Clear error specifically on txtUsername
            errLogin.SetError(txtUsername, string.Empty);

            // Keep label accurate if password error is still pending
            if (!string.IsNullOrEmpty(errLogin.GetError(txtPassword)))
            {
                lblLoginError.Text = "Please enter your password.";
            }
            else
            {
                lblLoginError.Text = string.Empty;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Clear error specifically on txtPassword
            errLogin.SetError(txtPassword, string.Empty);

            // Keep label accurate if username error is still pending
            if (!string.IsNullOrEmpty(errLogin.GetError(txtUsername)))
            {
                lblLoginError.Text = "Please enter your email or username.";
            }
            else
            {
                lblLoginError.Text = string.Empty;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
