namespace SoftBeckhoff.Structs
{
    public enum AdsDatatypeId : uint
    {
        /// <summary>Empty Type</summary>
        ADST_VOID = 0,
        /// <summary>Integer 16 Bit</summary>
        ADST_INT16 = 2,
        /// <summary>Integer 32 Bit</summary>
        ADST_INT32 = 3,
        /// <summary>Real (32 Bit)</summary>
        ADST_REAL32 = 4,
        /// <summary>Real 64 Bit</summary>
        ADST_REAL64 = 5,
        /// <summary>Integer 8 Bit</summary>
        ADST_INT8 = 16, // 0x00000010
        /// <summary>Unsigned integer 8 Bit</summary>
        ADST_UINT8 = 17, // 0x00000011
        /// <summary>Unsigned integer 16 Bit</summary>
        ADST_UINT16 = 18, // 0x00000012
        /// <summary>Unsigned Integer 32 Bit</summary>
        ADST_UINT32 = 19, // 0x00000013
        /// <summary>LONG Integer 64 Bit</summary>
        ADST_INT64 = 20, // 0x00000014
        /// <summary>Unsigned Long integer 64 Bit</summary>
        ADST_UINT64 = 21, // 0x00000015
        /// <summary>STRING</summary>
        ADST_STRING = 30, // 0x0000001E
        /// <summary>WSTRING</summary>
        ADST_WSTRING = 31, // 0x0000001F
        /// <summary>ADS REAL80</summary>
        ADST_REAL80 = 32, // 0x00000020
        /// <summary>ADS BIT</summary>
        ADST_BIT = 33, // 0x00000021
        /// <summary>Internal Only</summary>
        ADST_MAXTYPES = 34, // 0x00000022
        /// <summary>Blob</summary>
        ADST_BIGTYPE = 65, // 0x00000041
    }
}