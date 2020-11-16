using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ReadWriteRequestData
    {
        [MarshalAs(UnmanagedType.U4)]public uint IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public uint Offset;
        [MarshalAs(UnmanagedType.U4)]public uint ReadLenght;
        [MarshalAs(UnmanagedType.U4)]public uint WriteLenght;

        public override string ToString()
        {
            return $"{nameof(IndexGroup)}:{IndexGroup}, {nameof(Offset)}:{Offset}, {nameof(ReadLenght)}:{ReadLenght}, {nameof(WriteLenght)}:{WriteLenght}";
        }
    }
}