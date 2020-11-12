using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ReadRequestData
    {
        [MarshalAs(UnmanagedType.U4)]public int IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public int Offset;
        [MarshalAs(UnmanagedType.U4)]public int Lenght;

        public override string ToString()
        {
            return $"{nameof(IndexGroup)}:{IndexGroup}, {nameof(Offset)}:{Offset}, {nameof(Lenght)}:{Lenght}";
        }
    }
}