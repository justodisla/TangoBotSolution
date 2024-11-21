namespace TangoBotAPI.Toolkit
{
    public static class Constants
    {
        // API Base URLs
        public const string PRODUCTION_URL = "production_url";
        public const string SANDBOX_URL = "sandbox_url";
        //public const string CUSTOMERS_ENDPOINT = SANDBOX_URL + "/customers/me";
        //public const string ACCOUNTS_ENDPOINT = SANDBOX_URL + "/accounts";

        // Mock/Default Data for Testing
        public const string DEFAULT_ACCOUNT_NUMBER = "5WU34986";
        public const string DEFAULT_CUSTOMER_ID = "me";
        public const string DEFAULT_CUSTOMER_FIRST_NAME = "Sand";
        public const string DEFAULT_CUSTOMER_LAST_NAME = "Box";
        public const string DEFAULT_CUSTOMER_EMAIL = "jdisla@gmail.com";
        public const string DEFAULT_CUSTOMER_MOBILE = "1-809-757-0665";

        // Mock/Default Data for Account Testing
        public const string DEFAULT_INVESTMENT_OBJECTIVE = "SPECULATION";
        public const string DEFAULT_ACCOUNT_TYPE_NAME = "Individual";
        public const bool DEFAULT_DAY_TRADER_STATUS = false;

        // Other Constants
        public const int MAX_RETRY_ATTEMPTS = 3;
        public const int TOKEN_EXPIRATION_HOURS = 24;
        public const string JSON_CONTENT_TYPE = "application/json";

        // API Routes
        /*
        public static string GetCustomerAccountDetailsUrl(string accountNumber)
        {
            return $"{ACCOUNTS_ENDPOINT}/{accountNumber}";
        }

        public static string GetCustomerAccountBalanceUrl(string accountNumber)
        {
            return $"{ACCOUNTS_ENDPOINT}/{accountNumber}/balances";
        }

        public static string GetBalanceSnapshotsUrl(string accountNumber)
        {
            return $"{ACCOUNTS_ENDPOINT}/{accountNumber}/balance-snapshots";
        }

        public static string GetPositionsUrl(string accountNumber)
        {
            return $"{ACCOUNTS_ENDPOINT}/{accountNumber}/positions";
        }
        */
    }
}
