using System;
using System.Runtime.InteropServices;
using System.Text;
using SoftBeckhoff.Enums;
using SoftBeckhoff.Extensions;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsDataTypeEntry
    {
        [MarshalAs(UnmanagedType.U4)] public uint EntryLength;
        [MarshalAs(UnmanagedType.U4)] public uint Version;
        [MarshalAs(UnmanagedType.U4)] public uint HashValue;
        [MarshalAs(UnmanagedType.U4)] public uint TypeHashValue;
        [MarshalAs(UnmanagedType.U4)] public uint Size;
        [MarshalAs(UnmanagedType.U4)] public uint Offset;
        [MarshalAs(UnmanagedType.U4)] public AdsDatatypeId BaseTypeId;
        [MarshalAs(UnmanagedType.U4)] public AdsDataTypeFlags Flags;
        [MarshalAs(UnmanagedType.U2)] public ushort NameLength;
        [MarshalAs(UnmanagedType.U2)] public ushort TypeLength;
        [MarshalAs(UnmanagedType.U2)] public ushort CommentLength;
        [MarshalAs(UnmanagedType.U2)] public ushort ArrayDim;
        [MarshalAs(UnmanagedType.U2)] public ushort SubItems;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] EntryName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] TypeName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)] public byte[] Comment;

        public AdsDataTypeEntry(Type t)
        {
            EntryLength = (uint) new AdsDataTypeEntry().GetSize();
            Version = 1;
            HashValue = 1;
            TypeHashValue = 10;
            Size = 1;
            Offset = 0;
            BaseTypeId = AdsDatatypeId.ADST_UINT8;
            Flags = AdsDataTypeFlags.None;
            ArrayDim = 0;
            NameLength = 4;
            TypeLength = 4;
            CommentLength = 0;
            SubItems = 0;

            EntryName = new byte[5];
            TypeName = new byte[5];
            Comment = new byte[81];

            var typeBytes = Encoding.ASCII.GetBytes("BYTE");
            Array.Copy(typeBytes, EntryName, typeBytes.Length);
            Array.Copy(typeBytes, TypeName, typeBytes.Length);
            
        }
    }
}