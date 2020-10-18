namespace FutuCalc.Core.Symbols.Operators
{
    public class NegateOperator : UnaryOperator, IStackSymbol
    {
        public int Priority => 3;

        protected override double Operate(double operand)
        {
            return -operand;
        }
    }
}