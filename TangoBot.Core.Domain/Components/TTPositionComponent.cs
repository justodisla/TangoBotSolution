using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.App.DTOs;
using TangoBot.Core.Domain.DTOs;
using TangoBot.Core.Domain.Services;

namespace TangoBot.Core.Domain.Components
{
    public class TTPositionComponent : TTBaseApiComponent
    {
        public async Task<AccountPositionsDto?> GetAccountPositions(string accountNumber)
        {
            string endPoint = $"accounts/{accountNumber}/positions";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");

            var result = await ParseHttpResponseMessage<AccountPositionsDto>(response);

            return result;
        }
    }
}
