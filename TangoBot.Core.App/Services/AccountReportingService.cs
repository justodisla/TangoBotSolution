using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.App.DTOs;
using TangoBot.Core.Domain.Services;

namespace TangoBot.App.Services
{
    public class AccountReportingService
    {
        private readonly TastyTradeAccountComponent _accountComponent = new TastyTradeAccountComponent();
        public AccountBalanceDto GetAccountBalance(string account) {
            var accountBalances = _accountComponent.GetAccountBalancesAsync(account).Result;
            var accountBalanceDto = new AccountBalanceDto(accountBalances);
            return accountBalanceDto;
        }

        public AccountDto GetAccount(string account)
        {
            var accountData = _accountComponent.GetAccountAsync(account).Result;
            var accountDto = new AccountDto(accountData);
            return accountDto;
        }

        public AccountSnapShotDto GetAccountSnapShot(string account)
        {
            var accountSnapShot = _accountComponent.GetAccountSnapShotAsync(account).Result;
            var accountSnapShotDto = new AccountSnapShotDto(accountSnapShot);
            return accountSnapShotDto;
        }

        public CustomerDto GetCustomer()
        {
            var customerData = _accountComponent.GetCustomerAsync().Result;

            customerData.RootElement.GetProperty("accounts");

            var customerDto = new CustomerDto(customerData);
            return customerDto;
        }
    }
}
