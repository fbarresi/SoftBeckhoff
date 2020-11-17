using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ResponseHeader
    {
        [MarshalAs(UnmanagedType.U4)]public uint Result;

        public override string ToString()
        {
            return $"{nameof(Result)}:{Result}";
        }
    }
}