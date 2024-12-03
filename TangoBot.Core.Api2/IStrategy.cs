using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBot.Core.Api2
{
    public interface IStrategy
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        string Version { get; }

        List<IIndicator> indicators { get; }

        List<IRule> Rules { get; }

        void Initialize();

        void Optimize();

        public interface IRule
        {
            public enum RuleType
            {
                Entry,
                Exit,
                Hold
            }
            bool IsMet();

            RuleType Type { get; }

            string Description { get; }

            Dictionary<string, object> Parameters { get; }
        }

    }
}
