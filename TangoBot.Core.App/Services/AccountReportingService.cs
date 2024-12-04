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
        TTAccountComponent tTAccountComponent = new TTAccountComponent();
        public AccountBalanceDto GetAccountBalance(string account) {

            var bal = tTAccountComponent.GetAccountBalancesAsync(account);

            return new AccountBalanceDto(bal.Result);
        }
    }
}
