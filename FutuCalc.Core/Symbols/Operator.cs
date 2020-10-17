using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public abstract class Operator : IQueueSymbol
    {
        public void Process(ref Stack<double> values)
        {
            var firstValue = values.TryPop(out var x)
                ? x
                : 0;
            var secondValue = values.TryPop(out var y)
                ? y
                : 0;
            values.Push(Operate(secondValue, firstValue));
        }

        protected abstract double Operate(double first, double second);
    }
}