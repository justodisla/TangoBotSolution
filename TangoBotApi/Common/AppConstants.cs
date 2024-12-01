namespace TangoBotApi.Common
{
    /// <summary>
    /// Provides a centralized point to manage constants used solution-wide.
    /// </summary>
    public static class AppConstants
    {
        // Configuration keys
        public const string TEST_WEBSITE_URL = "TEST_WEBSITE_URL";
        public const string DATABASE_CONNECTION_STRING = "DATABASE_CONNECTION_STRING";
        public const string JWT_SECRET_KEY = "JWT_SECRET_KEY";

        // Other constants
        public const string DEFAULT_USER_ROLE = "User";
        public const string ADMIN_USER_ROLE = "Admin";
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
    }
}


