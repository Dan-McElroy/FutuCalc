namespace FutuCalc.Core.Symbols.Operators
{
    public class SubtractOperator : BinaryOperator, IStackSymbol
    {
        public int Priority => 1;

        protected override double Operate(double first, double second)
            => first - second;
    }
}