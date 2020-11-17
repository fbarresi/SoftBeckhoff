using System;
using System.Reflection;
using SoftBeckhoff.Enums;

namespace SoftBeckhoff.Extensions
{
    public static class TypeExtensions
    {
        public static AdsDatatypeId ToAdsDatatypeId(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int16:
                    return AdsDatatypeId.ADST_INT16;
                case TypeCode.UInt16:
                    return AdsDatatypeId.ADST_UINT16;
                case TypeCode.Int32:
                    return AdsDatatypeId.ADST_INT32;
                case TypeCode.UInt32:
                    return AdsDatatypeId.ADST_UINT32;
                case TypeCode.Int64:
                    return AdsDatatypeId.ADST_INT64;
                case TypeCode.UInt64:
                    return AdsDatatypeId.ADST_UINT64;
                case TypeCode.Single:
                    return AdsDatatypeId.ADST_REAL32;
                case TypeCode.Double:
                    return AdsDatatypeId.ADST_REAL64;
                case TypeCode.String:
                    return AdsDatatypeId.ADST_STRING;
                case TypeCode.Boolean:
                    return AdsDatatypeId.ADST_UINT8;
                case TypeCode.Byte:
                    return AdsDatatypeId.ADST_UINT8;
                default:
                    throw new ArgumentOutOfRangeException($"Type {type} not supported");
            }

        }
        
        public static string ToAdsDatatypeName(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int16:
                    return "INT";
                case TypeCode.UInt16:
                    return "UINT";
                case TypeCode.Int32:
                    return "DINT";
                case TypeCode.UInt32:
                    return "UDINT";
                case TypeCode.Int64:
                    return "LINT";
                case TypeCode.UInt64:
                    return "ULINT";
                case TypeCode.Single:
                    return "REAL";
                case TypeCode.Double:
                    return "LREAL";
                case TypeCode.String:
                    return "STRING(80)";
                case TypeCode.Boolean:
                    return "BOOL";
                case TypeCode.Byte:
                    return "BYTE";
                default:
                    throw new ArgumentOutOfRangeException($"Type {type} not supported");
            }

        }
    }
}