using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
