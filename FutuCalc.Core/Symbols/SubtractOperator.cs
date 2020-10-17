namespace FutuCalc.Core.Symbols
{
    public class SubtractOperator : Operator, IStackSymbol
    {
        public int Priority => 1;

        protected override double Operate(double first, double second)
            => first - second;
    }
}