using System;
using System.Collections.Generic;
using SoftBeckhoff.Models;

namespace SoftBeckhoff.Interfaces
{
    public interface IPlcService : IDisposable
    {
        IEnumerable<SymbolDto> GetSymbols();
        byte[] GetSymbol(string name);
        void SetSymbol(string name, byte[] value);
        void CreateSymbol(SymbolDto symbol);
    }
}