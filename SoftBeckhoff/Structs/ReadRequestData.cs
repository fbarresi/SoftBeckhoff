using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ReadRequestData
    {
        [MarshalAs(UnmanagedType.U4)]public uint IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public uint Offset;
        [MarshalAs(UnmanagedType.U4)]public uint Lenght;

        public override string ToString()
        {
            return $"{nameof(IndexGroup)}:{IndexGroup}, {nameof(Offset)}:{Offset}, {nameof(Lenght)}:{Lenght}";
        }
    }
}