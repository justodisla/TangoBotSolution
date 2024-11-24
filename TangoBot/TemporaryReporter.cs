﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotAPI.Streaming;

namespace TangoBot
{
    public class TemporaryReporter : IObserver<CandleEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Is complete");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("There was an error");
        }

        public void OnNext(CandleEvent value)
        {
            Console.WriteLine("\n\n_____________________________\nTANGOBOT TANGOBOT\n___________________________________________\n\n");
        }
    }
}