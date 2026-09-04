namespace EstateNexus;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        if (args != null && args.Length > 0 && args[0] == "--audit-model")
        {
            int code = EstateNexus.Tests.ModelAudit.RunAudit();
            Environment.Exit(code);
            return;
        }
        if (args != null && args.Length > 0 && args[0] == "--verify-integration")
        {
            int code = EstateNexus.Tests.ModelAudit.RunIntegrationVerification();
            Environment.Exit(code);
            return;
        }
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        // Initialize Database
        DatabaseSetup.InitializeDatabase();
        
        Application.Run(new LoginForm());
    }    
}
