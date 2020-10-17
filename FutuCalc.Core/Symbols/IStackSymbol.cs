namespace FutuCalc.Core.Symbols
{
    public interface ISymbol { }

    public interface IStackSymbol : ISymbol
    {
        int Priority { get; }
    }
}