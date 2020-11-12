using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsSymbolEntry
    {
        [MarshalAs(UnmanagedType.U4)]public int entryLength;
        [MarshalAs(UnmanagedType.U4)]public int indexGroup;
        [MarshalAs(UnmanagedType.U4)]public int indexOffset;
        [MarshalAs(UnmanagedType.U4)]public int size;
        [MarshalAs(UnmanagedType.U4)]public int dataType;
        [MarshalAs(UnmanagedType.U2)]public short flags;
        [MarshalAs(UnmanagedType.U2)]public short arrayDim;
        [MarshalAs(UnmanagedType.U2)]public short nameLength;
        [MarshalAs(UnmanagedType.U2)]public short typeLength;
        [MarshalAs(UnmanagedType.U2)]public short commentLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)] public byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)] public byte[] type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)] public byte[] comment;
        //array
        //guid
        //attributeCount
        //attributes
        //extended flags
        //reserve
    }
}