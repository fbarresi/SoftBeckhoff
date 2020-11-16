using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ResponseHeaderData
    {
        [MarshalAs(UnmanagedType.U4)]public uint Result;
        [MarshalAs(UnmanagedType.U4)]public uint Lenght;

        public override string ToString()
        {
            return $"{nameof(Result)}:{Result}, {nameof(Lenght)}:{Lenght}";
        }
    }
}