using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace Common.Shared.Helpers
{
    /// <summary>
    /// A helper class for managing password hashing and verification using the BCrypt algorithm.
    /// </summary>
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

        /// <summary>
        /// A method that takes a plain text password and returns a securely hashed version of it using the BCrypt algorithm.
        /// The method ensures that the input password is not null or empty before hashing, and throws an exception if the validation fails.
        /// </summary>
        /// <param name="password">Password to be hashed</param>
        /// <returns>A new hashed password</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Heslo nesmí být prázdné.");

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        /// <summary>
        /// A method that compares a plain text password with a previously hashed password to determine if they match.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>True if the two passwords match</returns>
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
