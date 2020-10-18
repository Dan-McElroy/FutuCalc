using System.Collections.Generic;
using FutuCalc.Core.Symbols;

namespace FutuCalc.Core.Calculation.Postfix
{
    public interface IPostfixConverter
    {
        /// <summary>
        /// Re-arranges a set of symbols into postfix notation.
        /// </summary>
        /// <param name="symbols">The infix set of symbols to be converted.</param>
        /// <returns>
        /// The given list of symbols in postfix notation, removing any symbols which
        /// are not directly operators or operands (i.e. brackets).
        /// </returns>
        IEnumerable<IQueueSymbol> ConvertToPostfix(IEnumerable<ISymbol> symbols);
    }
}