namespace EstateNexus;

partial class RegistrationForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblName = new System.Windows.Forms.Label();
        this.txtName = new System.Windows.Forms.TextBox();
        this.lblUsername = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblEmail = new System.Windows.Forms.Label();
        this.txtEmail = new System.Windows.Forms.TextBox();
        this.lblPhone = new System.Windows.Forms.Label();
        this.txtPhone = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblConfirmPassword = new System.Windows.Forms.Label();
        this.txtConfirmPassword = new System.Windows.Forms.TextBox();
        this.lblRole = new System.Windows.Forms.Label();
        this.cmbRole = new System.Windows.Forms.ComboBox();
        this.lblRegError = new System.Windows.Forms.Label();
        this.btnRegister = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.errRegistration = new System.Windows.Forms.ErrorProvider(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.errRegistration)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(140, 18);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(200, 30);
        this.lblTitle.Text = "Create an Account";

        // lblName
        this.lblName.AutoSize = true;
        this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblName.Location = new System.Drawing.Point(45, 65);
        this.lblName.Name = "lblName";
        this.lblName.Size = new System.Drawing.Size(73, 19);
        this.lblName.Text = "Full Name:";

        // txtName
        this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtName.Location = new System.Drawing.Point(175, 62);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(255, 25);

        // lblUsername
        this.lblUsername.AutoSize = true;
        this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblUsername.Location = new System.Drawing.Point(45, 105);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(74, 19);
        this.lblUsername.Text = "Username:";

        // txtUsername
        this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtUsername.Location = new System.Drawing.Point(175, 102);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(255, 25);

        // lblEmail
        this.lblEmail.AutoSize = true;
        this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblEmail.Location = new System.Drawing.Point(45, 145);
        this.lblEmail.Name = "lblEmail";
        this.lblEmail.Size = new System.Drawing.Size(44, 19);
        this.lblEmail.Text = "Email:";

        // txtEmail
        this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtEmail.Location = new System.Drawing.Point(175, 142);
        this.txtEmail.Name = "txtEmail";
        this.txtEmail.Size = new System.Drawing.Size(255, 25);

        // lblPhone
        this.lblPhone.AutoSize = true;
        this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblPhone.Location = new System.Drawing.Point(45, 185);
        this.lblPhone.Name = "lblPhone";
        this.lblPhone.Size = new System.Drawing.Size(51, 19);
        this.lblPhone.Text = "Phone:";

        // txtPhone
        this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtPhone.Location = new System.Drawing.Point(175, 182);
        this.txtPhone.Name = "txtPhone";
        this.txtPhone.Size = new System.Drawing.Size(255, 25);

        // lblPassword
        this.lblPassword.AutoSize = true;
        this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblPassword.Location = new System.Drawing.Point(45, 225);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(70, 19);
        this.lblPassword.Text = "Password:";

        // txtPassword
        this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtPassword.Location = new System.Drawing.Point(175, 222);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(255, 25);

        // lblConfirmPassword
        this.lblConfirmPassword.AutoSize = true;
        this.lblConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblConfirmPassword.Location = new System.Drawing.Point(45, 265);
        this.lblConfirmPassword.Name = "lblConfirmPassword";
        this.lblConfirmPassword.Size = new System.Drawing.Size(120, 19);
        this.lblConfirmPassword.Text = "Confirm Password:";

        // txtConfirmPassword
        this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtConfirmPassword.Location = new System.Drawing.Point(175, 262);
        this.txtConfirmPassword.Name = "txtConfirmPassword";
        this.txtConfirmPassword.PasswordChar = '*';
        this.txtConfirmPassword.Size = new System.Drawing.Size(255, 25);

        // lblRole
        this.lblRole.AutoSize = true;
        this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblRole.Location = new System.Drawing.Point(45, 305);
        this.lblRole.Name = "lblRole";
        this.lblRole.Size = new System.Drawing.Size(38, 19);
        this.lblRole.Text = "Role:";

        // cmbRole
        this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbRole.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.cmbRole.FormattingEnabled = true;
        this.cmbRole.Items.AddRange(new object[] { "Customer", "Admin" });
        this.cmbRole.Location = new System.Drawing.Point(175, 302);
        this.cmbRole.Name = "cmbRole";
        this.cmbRole.Size = new System.Drawing.Size(255, 25);

        // lblRegError
        this.lblRegError.AutoSize = true;
        this.lblRegError.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblRegError.ForeColor = System.Drawing.Color.Red;
        this.lblRegError.Location = new System.Drawing.Point(45, 340);
        this.lblRegError.Name = "lblRegError";
        this.lblRegError.Size = new System.Drawing.Size(0, 15);
        this.lblRegError.Text = "";

        // btnRegister
        this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.btnRegister.Location = new System.Drawing.Point(175, 365);
        this.btnRegister.Name = "btnRegister";
        this.btnRegister.Size = new System.Drawing.Size(120, 36);
        this.btnRegister.Text = "Register";
        this.btnRegister.UseVisualStyleBackColor = true;
        this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

        // btnCancel
        this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.btnCancel.Location = new System.Drawing.Point(310, 365);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(120, 36);
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        // RegistrationForm
        this.ClientSize = new System.Drawing.Size(480, 430);
        this.Controls.Add(this.lblRegError);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnRegister);
        this.Controls.Add(this.cmbRole);
        this.Controls.Add(this.lblRole);
        this.Controls.Add(this.txtConfirmPassword);
        this.Controls.Add(this.lblConfirmPassword);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtPhone);
        this.Controls.Add(this.lblPhone);
        this.Controls.Add(this.txtEmail);
        this.Controls.Add(this.lblEmail);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblUsername);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
        this.Controls.Add(this.lblTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "RegistrationForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "EstateNexus - Registration";
        ((System.ComponentModel.ISupportInitialize)(this.errRegistration)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblEmail;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblConfirmPassword;
    private System.Windows.Forms.TextBox txtConfirmPassword;
    private System.Windows.Forms.Label lblRole;
    private System.Windows.Forms.ComboBox cmbRole;
    private System.Windows.Forms.Label lblRegError;
    private System.Windows.Forms.ErrorProvider errRegistration;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.Button btnCancel;
}
