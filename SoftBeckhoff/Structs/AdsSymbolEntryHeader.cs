using System;
using System.Runtime.InteropServices;
using SoftBeckhoff.Enums;
using SoftBeckhoff.Extensions;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsSymbolEntryHeader
    {
        [MarshalAs(UnmanagedType.U4)]public uint EntryLength;
        [MarshalAs(UnmanagedType.U4)]public uint IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public uint IndexOffset;
        [MarshalAs(UnmanagedType.U4)]public uint Size;
        [MarshalAs(UnmanagedType.U4)]public AdsDatatypeId DataType;
        [MarshalAs(UnmanagedType.U2)]public AdsSymbolFlags Flags;
        [MarshalAs(UnmanagedType.U2)]public ushort ArrayDim;
        [MarshalAs(UnmanagedType.U2)]public ushort NameLength;
        [MarshalAs(UnmanagedType.U2)]public ushort TypeLength;
        [MarshalAs(UnmanagedType.U2)]public ushort CommentLength;
        
        public AdsSymbolEntryHeader(Type type)
        {
            EntryLength = 0;
            IndexGroup = 61445;
            IndexOffset = 0; //to supply
            Size = type == typeof(string) ? 81 : (uint) Marshal.SizeOf(type);
            DataType = type.ToAdsDatatypeId();
            Flags = AdsSymbolFlags.None;
            ArrayDim = 0;
            NameLength = 0; //to supply
            TypeLength = (ushort) type.ToAdsDatatypeName().Length;
            CommentLength = 0;

        }
    }
}