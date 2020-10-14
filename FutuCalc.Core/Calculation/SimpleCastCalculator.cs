namespace FutuCalc.Core.Calculation
{
    public class SimpleCastCalculator : ICalculator
    {
        public double Calculate(string equation)
        {
            return double.Parse(equation);
        }
    }
}