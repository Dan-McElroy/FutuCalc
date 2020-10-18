using System.Collections.Generic;

namespace FutuCalc.Core.Symbols.Operators
{
    public abstract class UnaryOperator : Operator
    {
        public override void Process(ref Stack<double> operands)
        {
            var firstValue = operands.Pop();
            operands.Push(Operate(firstValue));
        }

        protected abstract double Operate(double operand);
    }
}