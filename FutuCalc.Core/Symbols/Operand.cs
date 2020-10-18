using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public class Operand : IQueueSymbol
    {
        private readonly double _value;

        public Operand(double value)
        {
            _value = value;
        }

        public void Process(ref Stack<double> operands)
        {
            operands.Push(_value);
        }
    }
}