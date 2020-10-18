using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public interface IQueueSymbol : ISymbol
    {
        void Process(ref Stack<double> operands);
    }
}