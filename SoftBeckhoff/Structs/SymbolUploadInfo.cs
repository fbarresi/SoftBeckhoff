using System.Runtime.InteropServices;
using SoftBeckhoff.Extensions;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct SymbolUploadInfo
    {
        [MarshalAs(UnmanagedType.U4)]public int SymbolCount;
        [MarshalAs(UnmanagedType.U4)]public int SymbolsBlockSize;
        [MarshalAs(UnmanagedType.U4)]public int DataTypeCount;
        [MarshalAs(UnmanagedType.U4)]public int DataTypesBlockSize;
        [MarshalAs(UnmanagedType.U4)]public int MaxDynamicSymbolCount;
        [MarshalAs(UnmanagedType.U4)]public int UsedDynamicSymbolCount;
        [MarshalAs(UnmanagedType.U4)]public int InvalidDynamicSymbolCount;
        [MarshalAs(UnmanagedType.U4)]public int EncodingCodePage; //20127 // ASCII
        [MarshalAs(UnmanagedType.U4)]public int Flags; //0: None, 1: Is64BitPlatform, 2: IncludesBaseTypes
        [MarshalAs(UnmanagedType.U4)]public int Reserve1;
        [MarshalAs(UnmanagedType.U4)]public int Reserve2;
        [MarshalAs(UnmanagedType.U4)]public int Reserve3;
        [MarshalAs(UnmanagedType.U4)]public int Reserve4;
        [MarshalAs(UnmanagedType.U4)]public int Reserve5;
        [MarshalAs(UnmanagedType.U4)]public int Reserve6;
        [MarshalAs(UnmanagedType.U4)]public int Reserve7;

        public SymbolUploadInfo(int symbolCount, int symbolSize)
        {
            SymbolCount = symbolCount;
            SymbolsBlockSize = symbolSize;
            DataTypeCount = 0;
            DataTypesBlockSize = 0;//new AdsDataTypeEntry().GetSize();
            MaxDynamicSymbolCount = 0;
            UsedDynamicSymbolCount = 0;
            InvalidDynamicSymbolCount = 0;
            EncodingCodePage = 20127; // ASCII
            Reserve1 = 0;
            Reserve2 = 0;
            Reserve3 = 0;
            Reserve4 = 0;
            Reserve5 = 0;
            Reserve6 = 0;
            Reserve7 = 0;
            
            Flags = 1;
        }
    }
}