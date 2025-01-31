using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.App.DTOs;
using TangoBot.Core.Domain.DTOs;
using TangoBot.Core.Domain.Services;

namespace TangoBot.App.Services
{
    public class AccountCustomerReportingService
    {
        private readonly TTAccountCustomerComponent _accountComponent = new TTAccountCustomerComponent();
        public AccountBalanceDto GetAccountBalances(string account) {
            var accountBalances = _accountComponent.GetAccountBalancesAsync(account).Result;
            //var accountBalanceDto = new AccountBalanceDto(accountBalances);
            return accountBalances;
        }

        public AccountDto GetAccount(string account)
        {
            return _accountComponent.GetAccountAsync(account).Result;
           // var accountDto = new AccountDto(accountData);
           // return accountDto;
        }

        public AccountSnapShotsDto GetAccountSnapShots(string account)
        {
           return _accountComponent.GetAccountSnapShotsAsync(account).Result;
           // var accountSnapShotDto = new AccountSnapShotDto(accountSnapShot);
            //return accountSnapShotDto;
        }

        public CustomerDto? GetCustomer()
        {
            return _accountComponent.GetCustomerAsync().Result;

            //customerData.RootElement.GetProperty("accounts");

           // var customerDto = new CustomerDto(customerData);
            //return customerDto;
        }

        public CustomerAccountsDto? GetCustomerAccounts()
        {
            return _accountComponent.GetCustomerAccountsAsync().Result;
        }

        public AccountStatusDto GetAccountStatus(string account)
        {
            return _accountComponent.GetAccountActivityAsync(account).Result;
        }

        public AccountTransactionsDto? GetAccountTransactions(string account)
        {
            return _accountComponent.GetAccountTransactionsAsync(account).Result;
        }

        public AccountTransactionDto? GetAccountTransaction(string account, int transactionId)
        {
            return _accountComponent.GetAccountTransactionAsync(account, transactionId).Result;
        }

        public TotalFeesDto? GetTotalFees(string account)
        {
            return _accountComponent.GetTotalFeesAsync(account).Result;
        }
    }
}
