using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.Core.Domain.ValueObjects;

namespace TangoBot.App.DTOs
{
    public class AccountBalanceDto
    {
        public float Balance { get; private set; }

        public AccountBalanceDto(AccountBalances balance)
        {
            Balance = balance.Balance;
        }
    }
}
