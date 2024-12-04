using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.Core.Domain.ValueObjects;

namespace TangoBot.Core.Domain.Services
{
    public class TastyTradeAccountComponent : BaseApiComponent
    {
        public async Task<JsonDocument> GetAccountBalancesAsync(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/balances";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            return ParseHttpResponseMessage(response);
        }

        public async Task<JsonDocument> GetAccountAsync(string accountNumber)
        {
            string endPoint = $"customers/me/accounts/{accountNumber}";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            return ParseHttpResponseMessage(response);
        }


    }
}
