using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseSetup.ConnectionString))
                {
                    string query = "SELECT UserId, FullName, Username, Role, AccountStatus FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["AccountStatus"].ToString();
                                if (status == "Inactive")
                                {
                                    MessageBox.Show("Your account is inactive. Please contact admin.");
                                    return;
                                }

                                Session.UserId = Convert.ToInt32(reader["UserId"]);
                                Session.FullName = reader["FullName"].ToString();
                                Session.Username = reader["Username"]?.ToString() ?? username;
                                Session.Role = reader["Role"].ToString();

                                MessageBox.Show("Login Successful! Welcome " + Session.FullName);
                                
                                this.Hide();
                                
                                // Open appropriate dashboard based on role
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
                                MessageBox.Show("Invalid username or password.");
                            }
                        }
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
