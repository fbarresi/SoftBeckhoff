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
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct ReadWriteRequestData
    {
        [MarshalAs(UnmanagedType.U4)]public int IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public int Offset;
        [MarshalAs(UnmanagedType.U4)]public int ReadLenght;
        [MarshalAs(UnmanagedType.U4)]public int WriteLenght;

        public override string ToString()
        {
            return $"{nameof(IndexGroup)}:{IndexGroup}, {nameof(Offset)}:{Offset}, {nameof(ReadLenght)}:{ReadLenght}, {nameof(WriteLenght)}:{WriteLenght}";
        }
    }
}