using System.Collections;
using System.Collections.Generic;

namespace FutuCalc.Core.Calculation
{
    internal class Equation
    {
        private readonly IEnumerable<char> _tokens;

        public Equation(IEnumerable<char> tokens)
        {
            _tokens = tokens;
        }
    }
}