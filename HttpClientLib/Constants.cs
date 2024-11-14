namespace TangoBot
{
    public static class Constants
    {
        // API Base URLs
        public const string BaseApiUrl = "https://api.cert.tastyworks.com";
        public const string CustomersEndpoint = BaseApiUrl + "/customers/me";
        public const string AccountsEndpoint = BaseApiUrl + "/accounts";

        // Mock/Default Data for Testing
        public const string DefaultAccountNumber = "123456";
        public const string DefaultCustomerId = "me";
        public const string DefaultCustomerFirstName = "John";
        public const string DefaultCustomerLastName = "Doe";
        public const string DefaultCustomerEmail = "john.doe@example.com";
        public const string DefaultCustomerMobile = "123-456-7890";

        // Mock/Default Data for Account Testing
        public const string DefaultInvestmentObjective = "SPECULATION";
        public const string DefaultAccountTypeName = "Individual";
        public const bool DefaultDayTraderStatus = false;

        // Other Constants
        public const int MaxRetryAttempts = 3;
        public const int TokenExpirationHours = 24;
        public const string JsonContentType = "application/json";

        // API Routes
        public static string GetCustomerAccountDetailsUrl(string accountNumber)
        {
            return $"{AccountsEndpoint}/{accountNumber}";
        }

        public static string GetCustomerAccountBalanceUrl(string accountNumber)
        {
            return $"{AccountsEndpoint}/{accountNumber}/balances";
        }

        public static string GetBalanceSnapshotsUrl(string accountNumber)
        {
            return $"{AccountsEndpoint}/{accountNumber}/balance-snapshots";
        }

        public static string GetPositionsUrl(string accountNumber)
        {
            return $"{AccountsEndpoint}/{accountNumber}/positions";
        }
    }
}
