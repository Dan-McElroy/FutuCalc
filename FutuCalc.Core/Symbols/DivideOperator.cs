namespace FutuCalc.Core.Symbols
{
    public class DivideOperator : Operator, IStackSymbol
    {
        public int Priority => 2;

        protected override double Operate(double first, double second)
            => first / second;
    }
}