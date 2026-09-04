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

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email/username and password.");
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

                        if (user.AccountStatus == "Inactive" || !user.IsActive)
                        {
                            MessageBox.Show("Your account is inactive. Please contact admin.");
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
