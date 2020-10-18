﻿namespace FutuCalc.Core.Symbols.Operators
{
    public class AddOperator : BinaryOperator, IStackSymbol
    {
        public int Priority => 1;

        protected override double Operate(double first, double second)
            => first + second;
    }
}