using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public abstract class Operator : IQueueSymbol
    {
        public void Process(ref Stack<double> values)
        {
            values.Push(Operate(values.Pop(), values.Pop()));
        }

        protected abstract double Operate(double first, double second);
    }
}