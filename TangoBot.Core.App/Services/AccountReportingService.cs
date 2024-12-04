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
        TastyTradeAccountComponent tTAccountComponent = new TastyTradeAccountComponent();
        public AccountBalanceDto GetAccountBalance(string account) {
            var accountBalances = tTAccountComponent.GetAccountBalancesAsync(account).Result;
            var accountBalanceDto = new AccountBalanceDto(accountBalances);
            return accountBalanceDto;
        }

        public AccountDto GetAccount(string account)
        {
            var accountData = tTAccountComponent.GetAccountAsync(account).Result;
            var accountDto = new AccountDto(accountData);
            return accountDto;
        }
    }
}
