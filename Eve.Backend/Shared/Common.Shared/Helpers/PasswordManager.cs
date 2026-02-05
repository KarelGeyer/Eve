using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace Common.Shared.Helpers
{
    public class PasswordManager
    {
        private static int WorkFactor;

        public PasswordManager()
        {
            if (!int.TryParse(Resources.Global.WorkFaktor, out int workFactor))
            {
                workFactor = 12;
            }

            WorkFactor = workFactor;
        }

        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Heslo nesmí být prázdné.");

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
