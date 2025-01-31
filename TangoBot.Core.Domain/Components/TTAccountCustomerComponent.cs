using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TangoBot.App.DTOs;
using TangoBot.Core.Domain.DTOs;
using TangoBotApi.Services.DI;

namespace TangoBot.Core.Domain.Services
{
    public class TTAccountCustomerComponent : TTBaseApiComponent
    {
        public async Task<AccountBalanceDto?> GetAccountBalancesAsync(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/balances";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            var result = await ParseHttpResponseMessage<AccountBalanceDto>(response);

            return result;

            //return await ParseHttpResponseMessage<AccountBalanceDto>(response);
        }

        public async Task<AccountDto?> GetAccountAsync(string accountNumber)
        {
            string endPoint = $"customers/me/accounts/{accountNumber}";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            return await ParseHttpResponseMessage<AccountDto>(response);
        }

        public async Task<AccountSnapShotsDto?> GetAccountSnapShotsAsync(string account)
        {
            string endPoint = $"accounts/{account}/balance-snapshots";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            return await ParseHttpResponseMessage<AccountSnapShotsDto>(response);
        }

        public async Task<CustomerDto?> GetCustomerAsync()
        {
            string endPoint = "customers/me";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<CustomerDto>(response);
        }

        public async Task<CustomerAccountsDto?> GetCustomerAccountsAsync()
        {
            string endPoint = "customers/me/accounts";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<CustomerAccountsDto>(response);
        }

        public async Task<AccountStatusDto?> GetAccountActivityAsync(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/trading-status";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<AccountStatusDto>(response);
        }

        public async Task<AccountTransactionsDto?> GetAccountTransactionsAsync(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/transactions";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<AccountTransactionsDto>(response);
        }
    }
}
