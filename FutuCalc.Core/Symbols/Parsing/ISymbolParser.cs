using System.Collections.Generic;

namespace FutuCalc.Core.Symbols.Parsing
{
    public interface ISymbolParser
    {
        IEnumerable<ISymbol> ParseAsSymbols(string equation);
    }
}