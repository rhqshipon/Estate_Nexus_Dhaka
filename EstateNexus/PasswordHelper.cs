using System;
using System.Security.Cryptography;
using System.Text;

namespace EstateNexus
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes a plain-text password using SHA-256 and returns a 64-character lowercase hex string.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verifies an input password against the stored password hash.
        /// Also supports fallback comparison if the stored password was legacy plain text.
        /// </summary>
        public static bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedPasswordHash))
                return false;

            // 1. Compare against SHA-256 hash
            string inputHash = HashPassword(inputPassword);
            if (string.Equals(inputHash, storedPasswordHash, StringComparison.OrdinalIgnoreCase))
                return true;

            // 2. Legacy fallback for existing plain text records
            if (string.Equals(inputPassword, storedPasswordHash, StringComparison.Ordinal))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if a stored password string is already a 64-character SHA-256 hex string.
        /// </summary>
        public static bool IsHashed(string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword) || storedPassword.Length != 64)
                return false;

            for (int i = 0; i < storedPassword.Length; i++)
            {
                char c = storedPassword[i];
                bool isHex = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
                if (!isHex) return false;
            }
            return true;
        }
    }
}
