using System;
using System.Collections.Generic;

public interface IPlcService : IDisposable
{
    IEnumerable<object> GetSymbols();
    object GetSymbol(string name);
    void SetSymbol(string name, object value);
    void CreateSymbol(object symbol);
}