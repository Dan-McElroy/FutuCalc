using System;
using System.Collections.Generic;
using FutuCalc.Core.Symbols;
using FutuCalc.Core.Symbols.Parsing;

namespace FutuCalc.Core.Calculation.Postfix
{
    /// <summary>
    /// Calculates the result of equations via postfix or Reverse Polish notation.
    /// </summary>
    /// <remarks>
    /// Postfix notation is a representation of mathematical operations where the
    /// operator (i.e. +, -, *, /) immediately follows the values it operates on.
    /// For example, "3 + 4 * 2" becomes "3 4 2 * +" in postfix notation.
    /// </remarks>
    public class PostfixCalculator : ICalculator
    {
        private readonly ISymbolParser symbolParser;
        private readonly IPostfixConverter postfixConverter;

        public PostfixCalculator()
        {
            symbolParser = new SymbolParser();
            postfixConverter = new PostfixShuntingYardConverter();
        }

        public double Calculate(string equation)
        {
            var infixSymbols = symbolParser.ParseAsSymbols(equation);
            var postfixSymbols = postfixConverter.ConvertToPostfix(infixSymbols);
            return ProcessPostfixEquation(postfixSymbols);
        }

        /// <summary>
        /// Calculates the result of an equation from a list of symbols in postfix notation.
        /// </summary>
        /// <param name="symbols">The symbols in postfix notation to be calculated.</param>
        /// <returns>The result of the equation.</returns>
        /// <remarks>
        /// Uses a stack to keep track of the most recent operands in the list of symbols.
        /// When processing an operator in the symbol list, it operates on the most recent
        /// values in the stack.
        /// </remarks>
        /// <example>
        /// Given a set of symbols { 1, 3, +, 2, /, 5, * }:
        ///
        /// 1 and 3 is added to the stack: 3, 1
        /// + pops off 3 and 1, adds them and replaces them on the stack with the result (4): 4
        /// 2 is added to the stack: 2, 4
        /// / would pop off 2 and 4, divide 4 by 2 and replace them on the stack: 2
        /// 5 would be added to the stack: 5, 2
        /// * would pop off 5 and 2, multiply them and replace them on the stack: 10
        /// With no more symbols left to process and a stack of size 1, we return this last value on the stack as the result: 10.
        /// </example>
        internal static double ProcessPostfixEquation(IEnumerable<IQueueSymbol> symbols)
        {
            var values = new Stack<double>();
            foreach (var symbol in symbols)
            {
                symbol.Process(ref values);
            }

            if (values.Count > 1)
            {
                throw new InvalidOperationException("Postfix notation contains too many operands.");
            }
            return values.Pop();
        }
    }
}