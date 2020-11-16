using System;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Text;
using SoftBeckhoff.Enums;
using SoftBeckhoff.Extensions;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsSymbolEntry
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)] public byte[] Comment;
        //array
        //guid
        //attributeCount
        //attributes
        //extended flags
        //reserve

        public AdsSymbolEntry(Unit unit)
        {
            EntryLength = (uint) new AdsSymbolEntry().GetSize();
            IndexGroup = 61472;
            IndexOffset = 0;
            Size = 1;
            DataType = AdsDatatypeId.ADST_UINT8;
            Flags = AdsSymbolFlags.None;
            ArrayDim = 0;
            NameLength = 4;
            TypeLength = 4;
            CommentLength = 0;

            Name = new byte[5];
            Type = new byte[5];
            Comment = new byte[81];

            var nameBytes = Encoding.ASCII.GetBytes("Test");
            var typeBytes = Encoding.ASCII.GetBytes("BYTE");
            Array.Copy(nameBytes, Name, nameBytes.Length);
            Array.Copy(typeBytes, Type, typeBytes.Length);
        }
    }
}