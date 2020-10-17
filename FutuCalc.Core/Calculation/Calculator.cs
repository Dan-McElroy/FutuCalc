using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FutuCalc.Core.Symbols;

namespace FutuCalc.Core.Calculation
{
    public class Calculator : ICalculator
    {
        private static readonly char[] operators = { '+', '-', '*', '/', '(', ')' };

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
            => !Regex.IsMatch(equation, @"^(\d+|\(|\)|\*|\-|\+|\/| )*$");


        internal static IEnumerable<ISymbol> ConvertEquationToSymbolSeries(string equation)
        {
            var symbolStrings = new List<string>();

            var currentSymbol = string.Empty;
            var processingOperand = true;
            foreach (var token in equation.ToCharArray())
            {
                // if space, wrap up the last symbol if not empty and move on
                // if digit or punctuation, add to currentSymbol
                // if token, wrap up the last symbol if not empty, then add this one, and move on
                if ((char.IsWhiteSpace(token) || operators.Contains(token)) && !string.IsNullOrEmpty(currentSymbol))
                {
                    symbolStrings.Add(currentSymbol);
                    currentSymbol = string.Empty;
                }

                if (operators.Contains(token) || char.IsDigit(token) || char.IsPunctuation(token))
                {
                    currentSymbol += token;
                }
            }
            return symbolStrings.Select(ParseSymbol);
        }

        internal static ISymbol ParseSymbol(string token)
        {
            if (double.TryParse(token, out var value))
            {
                return new Operand(value);
            }
            if (token.Length > 1)
            {
                throw new ArgumentException($"Non-numerical symbols can only be one character long, but found: {token}", nameof(token));
            }
            return token[0] switch
            {
                '+' => new AddOperator(),
                '-' => new SubtractOperator(),
                '/' => new DivideOperator(),
                '*' => new MultiplyOperator(),
                '(' => new OpenBracket(),
                ')' => new CloseBracket(),
                _ => throw new ArgumentException(
                    $"An invalid character was found while parsing the equation: {token[0]}", nameof(token))
            };
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
                        continue;
                    case OpenBracket bracket:
                        symbolStack.Push(bracket);
                        continue;
                    case CloseBracket _:
                    {
                        while (symbolStack.Peek() is IQueueSymbol)
                        {
                            symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
                        }

                        symbolStack.Pop();
                        continue;
                    }
                    case IStackSymbol stackSymbol:
                    {
                        while (symbolStack.Any() && symbolStack.Peek().Priority > stackSymbol.Priority)
                        {
                            symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
                        }
                        symbolStack.Push(stackSymbol);
                        continue;
                    }
                }
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