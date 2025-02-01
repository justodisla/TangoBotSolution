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
    public class TTInstrumentsComponent : TTBaseApiComponent
    {
        public TTInstrumentsComponent() { }

        public async Task<InstrumentsDto?> GetEquitiesAsync()
        {
            string endPoint = $"/instruments/equities";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<InstrumentsDto>(response);
        }

        public async Task<InstrumentsDto?> GetEquitiesActiveAsync()
        {
            string endPoint = $"/instruments/equities/active";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<InstrumentsDto>(response);
        }

        public async Task<InstrumentDto?> GetEquityAsync(string symbol)
        {
            string endPoint = $"/instruments/equities/{symbol}";
            var response = await SendRequestAsync(endPoint, HttpMethod.Get) ?? throw new Exception("Response is null");
            return await ParseHttpResponseMessage<InstrumentDto>(response);
        }
    }
}
