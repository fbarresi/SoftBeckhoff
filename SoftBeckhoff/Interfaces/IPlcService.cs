using System;
using System.Collections.Generic;

namespace SoftBeckhoff.Interfaces
{
    public interface IPlcService : IDisposable
    {
        IEnumerable<object> GetSymbols();
        object GetSymbol(string name);
        void SetSymbol(string name, object value);
        void CreateSymbol(object symbol);
    }
}