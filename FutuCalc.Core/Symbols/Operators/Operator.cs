using System.Collections.Generic;

namespace FutuCalc.Core.Symbols.Operators
{
    /// <summary>
    /// Manipulates a set of values in a stack by replacing its topmost elements
    /// with the result of an operation on those elements.
    /// </summary>
    public abstract class Operator : IQueueSymbol
    {
        public abstract void Process(ref Stack<double> operands);
    }
}