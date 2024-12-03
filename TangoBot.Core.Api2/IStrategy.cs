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
        string Symbol { get; }
        int BackTrace { get; }
        int PeriodUnit { get; }
        int Period { get; }
        List<IIndicator> Indicators { get; }
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

            bool IsMet(IStrategyContext context);

            RuleType Type { get; }
            string Description { get; }
            Dictionary<string, object> Parameters { get; }

            /// <summary>
            /// List of rule validators that will be used to validate the rule
            /// DLL RV contain <path>dllPath</path> It must have one class that implements IRuleValidator (receives a json strategyContext and returns boolean).
            /// script contains <script lang="javascript">script</script>
            /// The main method of the script must be named "validate", must receive a json strategyContext and must return a boolean.
            /// </summary>
            string[] RuleValidators { get; }
        }
    }

    /// <summary>
    /// For dll rule validators implementations
    /// </summary>
    public interface IRuleValidator
    {
        bool Validate(IStrategyContext context, Dictionary<string, object> parameters);
    }

    public interface IStrategyContext
    {
        /// <summary>
        /// Serializes the stratey context into a json string
        /// </summary>
        /// <returns></returns>
        string Serialize();
    }
}

