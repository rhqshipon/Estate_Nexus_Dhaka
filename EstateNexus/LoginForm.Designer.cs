namespace EstateNexus;

partial class LoginForm
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
        this.lblUsername = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.chkShowPassword = new System.Windows.Forms.CheckBox();
        this.lblLoginError = new System.Windows.Forms.Label();
        this.btnLogin = new System.Windows.Forms.Button();
        this.btnClear = new System.Windows.Forms.Button();
        this.btnRegister = new System.Windows.Forms.Button();
        this.errLogin = new System.Windows.Forms.ErrorProvider(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.errLogin)).BeginInit();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(170, 35);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(155, 32);
        this.lblTitle.Text = "EstateNexus";

        // lblUsername
        this.lblUsername.AutoSize = true;
        this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblUsername.Location = new System.Drawing.Point(45, 105);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(121, 19);
        this.lblUsername.Text = "Email / Username:";

        // txtUsername
        this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtUsername.Location = new System.Drawing.Point(170, 102);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(230, 25);
        this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);

        // lblPassword
        this.lblPassword.AutoSize = true;
        this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.lblPassword.Location = new System.Drawing.Point(85, 155);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(70, 19);
        this.lblPassword.Text = "Password:";

        // txtPassword
        this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtPassword.Location = new System.Drawing.Point(170, 152);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(230, 25);
        this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);

        // chkShowPassword
        this.chkShowPassword.AutoSize = true;
        this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.chkShowPassword.Location = new System.Drawing.Point(170, 185);
        this.chkShowPassword.Name = "chkShowPassword";
        this.chkShowPassword.Size = new System.Drawing.Size(108, 19);
        this.chkShowPassword.Text = "Show Password";
        this.chkShowPassword.UseVisualStyleBackColor = true;
        this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);

        // lblLoginError
        this.lblLoginError.AutoSize = true;
        this.lblLoginError.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblLoginError.ForeColor = System.Drawing.Color.Red;
        this.lblLoginError.Location = new System.Drawing.Point(170, 212);
        this.lblLoginError.Name = "lblLoginError";
        this.lblLoginError.Size = new System.Drawing.Size(0, 15);
        this.lblLoginError.Text = "";

        // btnLogin
        this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.btnLogin.Location = new System.Drawing.Point(140, 238);
        this.btnLogin.Name = "btnLogin";
        this.btnLogin.Size = new System.Drawing.Size(80, 36);
        this.btnLogin.Text = "Login";
        this.btnLogin.UseVisualStyleBackColor = true;
        this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

        // btnClear
        this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.btnClear.Location = new System.Drawing.Point(230, 238);
        this.btnClear.Name = "btnClear";
        this.btnClear.Size = new System.Drawing.Size(80, 36);
        this.btnClear.Text = "Clear";
        this.btnClear.UseVisualStyleBackColor = true;
        this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

        // btnRegister
        this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.btnRegister.Location = new System.Drawing.Point(320, 238);
        this.btnRegister.Name = "btnRegister";
        this.btnRegister.Size = new System.Drawing.Size(80, 36);
        this.btnRegister.Text = "Register";
        this.btnRegister.UseVisualStyleBackColor = true;
        this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

        // errLogin
        this.errLogin.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
        this.errLogin.ContainerControl = this;

        // LoginForm
        this.AcceptButton = this.btnLogin;
        this.ClientSize = new System.Drawing.Size(500, 320);
        this.Controls.Add(this.lblLoginError);
        this.Controls.Add(this.chkShowPassword);
        this.Controls.Add(this.btnClear);
        this.Controls.Add(this.btnRegister);
        this.Controls.Add(this.btnLogin);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblUsername);
        this.Controls.Add(this.lblTitle);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "LoginForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "EstateNexus - Login";
        ((System.ComponentModel.ISupportInitialize)(this.errLogin)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.CheckBox chkShowPassword;
    private System.Windows.Forms.Label lblLoginError;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.ErrorProvider errLogin;
}
