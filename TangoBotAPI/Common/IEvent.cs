﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotApi.Common
{
    public interface IEvent
    {
        Guid Id { get; }
        string Subject { get; }
    }
}
