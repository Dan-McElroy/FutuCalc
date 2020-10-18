using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FutuCalc.Core.Symbols;

namespace FutuCalc.Core.Calculation
{
    public class Calculator : ICalculator
    {
        private const string tokenFormat = @"\d+(\.\d+)?|\(|\)|\+|\-|\*|\/";
        private const string legalEquationFormat = @"^(" + tokenFormat + "| )+$";

        public double Calculate(string equation)
        {
            if (ContainsIllegalCharacters(equation))
            {
                throw new InvalidOperationException(
                    "Equation contains invalid characters. Numbers, brackets, spaces, +, -, * and / are all valid characters.");
            }

            var infixSymbols = ConvertEquationToSymbolSeries(equation);
            var postfixSymbols = ConvertSymbolSeriesToPostfix(infixSymbols);
            return ProcessPostfixEquation(postfixSymbols);
        }

        private static bool ContainsIllegalCharacters(string equation)
        {
            return !Regex.IsMatch(equation, legalEquationFormat);
        }

        internal static IEnumerable<ISymbol> ConvertEquationToSymbolSeries(string equation)
        {
            var symbols = new List<ISymbol>();

            var tokens = Regex.Matches(equation, tokenFormat)
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

        internal static ISymbol ParseSymbol(string token, bool unaryPossible)
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

        /// <summary>
        /// Implements Dijkstra's "Shunting Yard" algorithm to process a regular, infix
        /// equation to a postfix format.
        /// </summary>
        /// <param name="symbols">An infix equation represented as a series of symbols.</param>
        /// <returns></returns>
        internal static IEnumerable<IQueueSymbol> ConvertSymbolSeriesToPostfix(IEnumerable<ISymbol> symbols)
        {
            var symbolQueue = new Queue<IQueueSymbol>();
            var symbolStack = new Stack<IStackSymbol>();

            foreach (var symbol in symbols)
            {
                switch (symbol)
                {
                    case Operand operand:
                        symbolQueue.Enqueue(operand);
                        break;
                    case OpenBracket bracket:
                        symbolStack.Push(bracket);
                        break;
                    case CloseBracket _:
                    {
                        while (symbolStack.Peek() is IQueueSymbol)
                        {
                            symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
                        }
                        if (!symbolStack.TryPop(out _))
                        {
                            throw new InvalidOperationException("Non-matching number of brackets found in the equation.");
                        }
                        break;
                    }
                    case IStackSymbol stackSymbol:
                    {
                        while (symbolStack.TryPeek(out var topSymbol) && topSymbol.Priority >= stackSymbol.Priority)
                        {
                            symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
                        }
                        symbolStack.Push(stackSymbol);
                        break;
                    }
                }
            }
            if (symbolStack.OfType<OpenBracket>().Any())
            {
                throw new InvalidOperationException("Non-matching number of brackets found in the equation.");
            }
            while (symbolStack.Any())
            {
                symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
            }
            return symbolQueue;
        }

        internal static double ProcessPostfixEquation(IEnumerable<IQueueSymbol> symbols)
        {
            var values = new Stack<double>();
            foreach (var symbol in symbols)
            {
                symbol.Process(ref values);
            }
            return values.Sum();
        }


    }
}