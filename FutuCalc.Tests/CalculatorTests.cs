using System;
using FutuCalc.Core.Calculation;
using NUnit.Framework;

namespace FutuCalc.Tests
{
    public class CalculatorTests
    {
        private const double DeltaThreshold = 1E-5;

        private ICalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [TestCase("7", 7)]
        [TestCase("07", 7)]
        [TestCase("007", 7)]
        [TestCase("24", 24)]
        [TestCase("-7", -7)]
        [TestCase("- 7", -7)]
        [TestCase("-07", -7)]
        [TestCase("0", 0)]
        [TestCase("-0", 0)]
        [TestCase("00", 0)]
        public void CorrectCalculation_ForSingleNumber(string equation, double expected)
        {
            AssertCalculation(equation, expected);
        }

        [Test]
        public void CorrectCalculation_ForAddition()
        {
            AssertCalculation("2 + 3", 5);
        }

        [Test]
        public void CorrectCalculation_ForSubtraction()
        {
            AssertCalculation("2 - 3", -1);
        }

        [TestCase("2 * 3", 6)]
        [TestCase("2 * -3", -6)]
        [TestCase("-2 * 3", -6)]
        [TestCase("-2 * -3", 6)]
        public void CorrectCalculation_ForMultiplication(string equation, double expected)
        {
            AssertCalculation(equation, expected);
        }

        [TestCase("4 / 2", 2)]
        [TestCase("4 / -2", -2)]
        [TestCase("-4 / 2", -2)]
        [TestCase("-4 / -2", 2)]
        [TestCase("2 / 3", 2 / 3)]
        public void CorrectCalculation_ForDivision(string equation, double expected)
        {
            AssertCalculation(equation, expected);
        }

        [TestCase("3 * 4 + 1 / 2", 12.5)]
        [TestCase("3 / 4 * 1 + 3", 3)]
        [TestCase("6 + 4 / 3 - 1", 5)]
        public void CorrectCalculation_ForMultipleOperators(string equation, double expected)
        {
            AssertCalculation(equation, expected);
        }

        [TestCase("(3)", 3)]
        [TestCase("(3 * 2)", 6)]
        [TestCase("(3 + 2) * 2", 10)]
        [TestCase("(3 + (28 / 14)) * 2", 10)]
        public void CorrectCalculation_ForBrackets(string equation, double expected)
        {
            AssertCalculation(equation, expected);
        }

        [Test]
        public void CorrectCalculation_ForInconsistentSpacing(string equation, double expected)
        {
            AssertCalculation("3+3 + 3-1 - 1", 7);
        }

        [Test]
        public void CorrectCalculation_ForComplexEquation()
        {
            AssertCalculation("2 * (23/(3*3))- 23 * (2*3)", 2 * (23/(3*3))- 23 * (2*3));
        }

        [Test]
        public void ExceptionThrown_ForNullEquation()
        {
            Assert.Throws<NullReferenceException>(() => calculator.Calculate(null));
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase("()")]
        [TestCase("\t\n")]
        public void ExceptionThrown_ForBlankEquation(string query)
        {
            Assert.Throws<InvalidOperationException>(() => calculator.Calculate(query));
        }

        [TestCase("a")]
        [TestCase("7 % 2")]
        [TestCase("\"2\" + 1")]
        public void ExceptionThrown_ForInvalidCharacters(string query)
        {
            Assert.Throws<InvalidOperationException>(() => calculator.Calculate(query));
        }

        [TestCase("(3")]
        [TestCase("(3)(")]
        [TestCase("1 + (2 + (1 - 4)")]
        public void ExceptionThrown_ForIncompleteBrackets(string query)
        {
            Assert.Throws<InvalidOperationException>(() => calculator.Calculate(query));
        }

        private void AssertCalculation(string equation, double expected)
        {
            Assert.AreEqual(expected, calculator.Calculate(equation), DeltaThreshold);
        }
    }
}