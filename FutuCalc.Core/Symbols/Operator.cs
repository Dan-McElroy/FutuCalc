using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public abstract class Operator : IQueueSymbol
    {
        public abstract void Process(ref Stack<double> operands);
    }
}