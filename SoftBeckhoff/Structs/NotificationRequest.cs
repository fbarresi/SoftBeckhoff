using System.Runtime.InteropServices;

namespace SoftBeckhoff.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]public struct NotificationRequest
    {
        [MarshalAs(UnmanagedType.U4)]public uint IndexGroup;
        [MarshalAs(UnmanagedType.U4)]public uint IndexOffset;
        [MarshalAs(UnmanagedType.U4)]public uint Length;
        [MarshalAs(UnmanagedType.U4)]public uint TransmissionMode; //ignored
        [MarshalAs(UnmanagedType.U4)]public uint MaxDelay; //ignored
        [MarshalAs(UnmanagedType.U4)]public uint CycleTime;

        [MarshalAs(UnmanagedType.U4)]public uint Spare1;
        [MarshalAs(UnmanagedType.U4)]public uint Spare2;
        [MarshalAs(UnmanagedType.U4)]public uint Spare3;
        [MarshalAs(UnmanagedType.U4)]public uint Spare4;

    }
}