using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.App.DTOs;
using TangoBot.Core.Domain.Components;
using TangoBot.Core.Domain.DTOs;
using TangoBot.Core.Domain.Services;

namespace TangoBot.App.Services
{
    public class InstrumentService
    {
        private readonly TTInstrumentsComponent _instrumentComponent = new TTInstrumentsComponent();

        public async Task<InstrumentsDto?> GetInstrumentAsync()
        {
            return await _instrumentComponent.GetEquitiesAsync();
        }
    }
}
