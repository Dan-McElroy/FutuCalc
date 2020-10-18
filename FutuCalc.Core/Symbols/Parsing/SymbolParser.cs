using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FutuCalc.Core.Symbols.Operators;

namespace FutuCalc.Core.Symbols.Parsing
{
    internal class SymbolParser : ISymbolParser
    {
        private const string TokenFormat = @"\d+(\.\d+)?|\(|\)|\+|\-|\*|\/";
        private const string LegalEquationFormat = @"^(" + TokenFormat + "| )+$";

        public IEnumerable<ISymbol> ParseAsSymbols(string equation)
        {
            if (ContainsIllegalCharacters(equation))
            {
                throw new InvalidOperationException(
                    "Equation contains invalid characters. Numbers, brackets, spaces, +, -, * and / are all valid characters.");
            }

            var symbols = new List<ISymbol>();

            var tokens = Regex.Matches(equation, TokenFormat)
                .Select(t => t.Value);

            var unaryPossible = true;
            foreach (var token in tokens)
            {
                var symbol = ParseSymbol(token, unaryPossible);
                unaryPossible = symbol is BinaryOperator;
                symbols.Add(symbol);
            }

            return symbols;
        }

        private static bool ContainsIllegalCharacters(string equation)
        {
            return !Regex.IsMatch(equation, LegalEquationFormat);
        }

        private static ISymbol ParseSymbol(string token, bool unaryPossible)
        {
            if (double.TryParse(token, out var value))
            {
                return new Operand(value);
            }
            switch (token[0])
            {
                case '+':
                    return new AddOperator();
                case '-':
                {
                    if (unaryPossible)
                    {
                        return new NegateOperator();
                    }
                    return new SubtractOperator();
                }
                case '/':
                    return new DivideOperator();
                case '*':
                    return new MultiplyOperator();
                case '(':
                    return new OpenBracket();
                case ')':
                    return new CloseBracket();
                default:
                    throw new ArgumentException(
                        $"An invalid character was found while parsing the equation: {token[0]}", nameof(token));
            }
        }
    }
}