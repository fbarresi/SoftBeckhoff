using System;
using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsNotification
    {
        [MarshalAs(UnmanagedType.U4)] public uint Length; //overall size
        [MarshalAs(UnmanagedType.U4)] public uint Stamps; //1
        public AdsNotificationHeader AdsNotificationHeader;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsNotificationHeader
    {
        [MarshalAs(UnmanagedType.U8)] public long Timestamp; //DateTime.UtcNow.ToFileTime()
        [MarshalAs(UnmanagedType.U4)] public uint Samples; //1
        public AdsNotificationSample Sample;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct AdsNotificationSample
    {
        [MarshalAs(UnmanagedType.U4)] public uint Handle;
        [MarshalAs(UnmanagedType.U4)] public uint Size;
        //data
    }
}