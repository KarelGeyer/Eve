namespace Common.Shared.Exceptions
{
    public class ForbidenException : Exception
    {
        public string Email { get; }

        public ForbidenException(string email)
            : base($"Account with email '{email}' is not verified yet.")
        {
            Email = email;
        }
    }
}
