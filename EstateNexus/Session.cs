namespace EstateNexus
{
    public static class Session
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }

        public static void Logout()
        {
            UserId = 0;
            FullName = null;
            Role = null;
        }
    }
}

