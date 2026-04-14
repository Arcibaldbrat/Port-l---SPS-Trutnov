namespace Port_SPS.Services
{
    public interface IEmailValidator
    {
        bool IsValidSchoolEmail(string email);
        string GetEmailDomain();
    }

    public class SchoolEmailValidator : IEmailValidator
    {
        private const string SCHOOL_EMAIL_DOMAIN = "@spstrutnov.cz";

        public bool IsValidSchoolEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return email.EndsWith(SCHOOL_EMAIL_DOMAIN, StringComparison.OrdinalIgnoreCase);
        }

        public string GetEmailDomain()
        {
            return SCHOOL_EMAIL_DOMAIN;
        }
    }
}
