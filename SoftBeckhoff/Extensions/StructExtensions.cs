using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SoftBeckhoff.Extensions
{
  internal static class StructExtensions
  {
    public static T ByteArrayToStructure<T>(this byte[] bytes) where T : struct
    {
      GCHandle gcHandle = GCHandle.Alloc((object) bytes, GCHandleType.Pinned);
      try
      {
        return (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static T DeepSwap<T>(this T source) where T : struct => (T) StructExtensions.DeepSwap((object) source);

    public static byte[] GetBytes<T>(this T obj) where T : struct
    {
      int length = Marshal.SizeOf<T>(obj);
      IntPtr num = Marshal.AllocHGlobal(length);
      try
      {
        byte[] destination = new byte[length];
        Marshal.StructureToPtr<T>(obj, num, true);
        Marshal.Copy(num, destination, 0, length);
        return destination;
      }
      finally
      {
        Marshal.FreeHGlobal(num);
      }
    }

    public static int GetSize<T>(this T obj) where T : struct => Marshal.SizeOf<T>(obj);

    public static bool IsStruct(this Type type) => type.IsValueType && !type.IsEnum && !type.IsPrimitive;

    public static T Swap<T>(this T source) where T : struct => (T) StructExtensions.Swap((object) source);

    private static void CallRecursiveDeepSwapOnElementsOf(Array array)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        object source = array.GetValue(index);
        array.SetValue(StructExtensions.DeepSwap(source), index);
      }
    }

    private static Array CopyArray(Array array)
    {
      Array instance = Array.CreateInstance(array.GetType().GetElementType(), array.Length);
      array.CopyTo(instance, 0);
      return instance;
    }

    private static object DeepSwap(object source)
    {
      source = StructExtensions.Swap(source);
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) source.GetType().GetFields()).Where<FieldInfo>(
        (Func<FieldInfo, bool>) (f => f.GetValue(source) is Array)))
      {
        Array array = StructExtensions.CopyArray(fieldInfo.GetValue(source) as Array);
        StructExtensions.CallRecursiveDeepSwapOnElementsOf(array);
        fieldInfo.SetValue(source, (object) array);
      }

      return source;
    }

    private static object Swap(object source)
    {
      object obj = source;
      foreach (FieldInfo field in source.GetType().GetFields())
      {
        object source1 = field.GetValue(obj);
        switch (Type.GetTypeCode(source1.GetType()))
        {
          case TypeCode.Int16:
            short hostOrder1 = IPAddress.NetworkToHostOrder((short) source1);
            field.SetValue(obj, (object) hostOrder1);
            break;
          case TypeCode.UInt16:
            short hostOrder2 = IPAddress.NetworkToHostOrder((short) (ushort) source1);
            field.SetValue(obj, (object) (ushort) hostOrder2);
            break;
          case TypeCode.Int32:
            int hostOrder3 = IPAddress.NetworkToHostOrder((int) source1);
            field.SetValue(obj, (object) hostOrder3);
            break;
          case TypeCode.UInt32:
            int hostOrder4 = IPAddress.NetworkToHostOrder((int) (uint) source1);
            field.SetValue(obj, (object) (uint) hostOrder4);
            break;
          case TypeCode.Int64:
            long hostOrder5 = IPAddress.NetworkToHostOrder((long) source1);
            field.SetValue(obj, (object) hostOrder5);
            break;
          case TypeCode.UInt64:
            long hostOrder6 = IPAddress.NetworkToHostOrder((long) (ulong) source1);
            field.SetValue(obj, (object) (ulong) hostOrder6);
            break;
          case TypeCode.Single:
            float single = System.BitConverter.ToSingle(
              ((IEnumerable<byte>) System.BitConverter.GetBytes((float) source1)).Reverse<byte>().ToArray<byte>(), 0);
            field.SetValue(obj, (object) single);
            break;
          case TypeCode.Double:
            double num = System.BitConverter.ToDouble(
              ((IEnumerable<byte>) System.BitConverter.GetBytes((double) source1)).Reverse<byte>().ToArray<byte>(), 0);
            field.SetValue(obj, (object) num);
            break;
          default:
            if (source1.GetType().IsStruct())
            {
              field.SetValue(obj, StructExtensions.Swap(source1));
              break;
            }

            break;
        }
      }

      return obj;
    }
  }
}