using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.Core.Domain.Components;
using TangoBot.Core.Domain.DTOs;
using TangoBot.Core.Domain.Services;

namespace TangoBot.App.Services
{
    public class PositionService
    {
        private readonly TTPositionComponent _positionComponent = new TTPositionComponent();
        public PositionService() { }

        public AccountPositionsDto? GetAccountPositions(string accountNumber)
        {
            var result = _positionComponent.GetAccountPositions(accountNumber).Result;
            return result;
        }
    }
}
