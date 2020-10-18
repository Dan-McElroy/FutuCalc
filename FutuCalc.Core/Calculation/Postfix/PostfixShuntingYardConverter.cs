using System;
using System.Collections.Generic;
using System.Linq;
using FutuCalc.Core.Symbols;

namespace FutuCalc.Core.Calculation.Postfix
{
    /// <summary>
    /// Implements Dijkstra's "Shunting Yard" algorithm to process an infix
    /// equation to a postfix format.
    /// </summary>
    /// <remarks>
    /// Adds operands to a queue as it processes them, and uses a stack
    /// to prioritise operators (and group them according to brackets).
    /// </remarks>
    internal class PostfixShuntingYardConverter : IPostfixConverter
    {
        private Queue<IQueueSymbol> symbolQueue = new Queue<IQueueSymbol>();
        private Stack<IStackSymbol> symbolStack = new Stack<IStackSymbol>();
        public IEnumerable<IQueueSymbol> ConvertToPostfix(IEnumerable<ISymbol> symbols)
        {
            symbolQueue.Clear();
            symbolStack.Clear();

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
                        HandleClosedBracket();
                        break;
                    case IStackSymbol stackSymbol:
                        HandleStackSymbol(stackSymbol);
                        break;
                }
            }
            if (symbolStack.OfType<OpenBracket>().Any())
            {
                throw new InvalidOperationException("Non-matching number of brackets found in the equation.");
            }
            EmptyStack();
            return symbolQueue;
        }

        private void HandleClosedBracket()
        {
            while (symbolStack.Peek() is IQueueSymbol)
            {
                symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
            }
            if (!symbolStack.TryPop(out _))
            {
                throw new InvalidOperationException("Non-matching number of brackets found in the equation.");
            }
        }

        private void HandleStackSymbol(IStackSymbol stackSymbol)
        {
            while (symbolStack.TryPeek(out var topSymbol) && topSymbol.Priority >= stackSymbol.Priority)
            {
                symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
            }
            symbolStack.Push(stackSymbol);
        }

        private void EmptyStack()
        {
            while (symbolStack.Any())
            {
                symbolQueue.Enqueue(symbolStack.Pop() as IQueueSymbol);
            }
        }
    }
}