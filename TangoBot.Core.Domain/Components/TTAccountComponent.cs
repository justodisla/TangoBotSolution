using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.Core.Domain.ValueObjects;

namespace TangoBot.Core.Domain.Services
{
    public class TTAccountComponent : BaseApiComponent
    {
        public async Task<AccountBalances> GetAccountBalancesAsync(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/balances";
            await SendRequestAsync(endPoint, HttpMethod.Get);
            return null;
        }
    }
}
