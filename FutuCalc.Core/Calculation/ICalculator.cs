using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("FutuCalc.Tests")]
namespace FutuCalc.Core.Calculation
{
    public interface ICalculator
    {
        /// <summary>
        /// Calculates the result of an equation as represented by a
        /// string.
        /// </summary>
        /// <param name="equation">
        /// The equation to calculate. Valid characters include decimals,
        /// periods, spaces,
        /// <code>+</code>, <code>-</code>, <code>/</code>, <code>*</code>
        /// </param>
        /// <returns>The result of the equation.</returns>
        double Calculate(string equation);
    }
}