using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ResponseHeaderData
    {
        [MarshalAs(UnmanagedType.U4)]public int Result;
        [MarshalAs(UnmanagedType.U4)]public int Lenght;

        public override string ToString()
        {
            return $"{nameof(Result)}:{Result}, {nameof(Lenght)}:{Lenght}";
        }
    }
}