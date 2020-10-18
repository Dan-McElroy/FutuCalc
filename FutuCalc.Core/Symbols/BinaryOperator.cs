using System.Collections.Generic;

namespace FutuCalc.Core.Symbols
{
    public abstract class BinaryOperator : Operator
    {
        public override void Process(ref Stack<double> operands)
        {
            var firstOperand = operands.Pop();
            var secondOperand = operands.Pop();
            operands.Push(Operate(secondOperand, firstOperand));
        }

        protected abstract double Operate(double first, double second);
    }
}