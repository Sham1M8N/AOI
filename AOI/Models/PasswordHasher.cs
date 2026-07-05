using System;
using System.Security.Cryptography;

namespace AOI.Models
{
    // ponytail: legacy unsalted-SHA256 rows (no "PBKDF2$" prefix) still verify via Verify(),
    // and get transparently rehashed to the new format by the caller on next successful login.
    public static class PasswordHasher
    {
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            byte[] hash = DeriveKey(password, salt, Iterations);
            return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrEmpty(stored)) return false;

            if (!stored.StartsWith("PBKDF2$"))
                return string.Equals(LegacySha256(password), stored, StringComparison.OrdinalIgnoreCase);

            string[] parts = stored.Split('$');
            if (parts.Length != 4) return false;

            int iterations = int.Parse(parts[1]);
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expected = Convert.FromBase64String(parts[3]);
            byte[] actual = DeriveKey(password, salt, iterations);

            return FixedTimeEquals(actual, expected);
        }

        public static bool IsLegacyHash(string stored) => !string.IsNullOrEmpty(stored) && !stored.StartsWith("PBKDF2$");

        private static byte[] DeriveKey(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                return pbkdf2.GetBytes(HashSize);
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

        private static string LegacySha256(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                var builder = new System.Text.StringBuilder();
                foreach (var b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
