namespace SoftBeckhoff.Enums
{
    public enum AdsDataTypeFlags : uint
  {
    /// <summary>ADSDATATYPEFLAG_DATATYPE</summary>
    DataType = 1,
    /// <summary>ADSDATATYPEFLAG_DATAITEM</summary>
    DataItem = 2,
    /// <summary>ADSDATATYPEFLAG_REFERENCETO</summary>
    ReferenceTo = 4,
    /// <summary>ADSDATATYPEFLAG_METHODDEREF</summary>
    MethodDeref = 8,
    /// <summary>ADSDATATYPEFLAG_OVERSAMPLE</summary>
    Oversample = 16, // 0x00000010
    /// <summary>ADSDATATYPEFLAG_BITVALUES</summary>
    BitValues = 32, // 0x00000020
    /// <summary>ADSDATATYPEFLAG_PROPITEM</summary>
    PropItem = 64, // 0x00000040
    /// <summary>ADSDATATYPEFLAG_TYPEGUID</summary>
    TypeGuid = 128, // 0x00000080
    /// <summary>ADSDATATYPEFLAG_PERSISTENT</summary>
    Persistent = 256, // 0x00000100
    /// <summary>ADSDATATYPEFLAG_COPYMASK</summary>
    CopyMask = 512, // 0x00000200
    /// <summary>ADSDATATYPEFLAG_TCCOMIFACEPTR</summary>
    TComInterfacePtr = 1024, // 0x00000400
    /// <summary>ADSDATATYPEFLAG_METHODINFOS</summary>
    MethodInfos = 2048, // 0x00000800
    /// <summary>ADSDATATYPEFLAG_ATTRIBUTES</summary>
    Attributes = 4096, // 0x00001000
    /// <summary>ADSDATATYPEFLAG_ENUMINFOS</summary>
    EnumInfos = 8192, // 0x00002000
    /// <summary>
    /// this flag is set if the datatype is aligned (ADSDATATYPEFLAG_ALIGNED)
    /// </summary>
    Aligned = 65536, // 0x00010000
    /// <summary>
    /// data item is static - do not use offs (ADSDATATYPEFLAG_STATIC)
    /// </summary>
    Static = 131072, // 0x00020000
    /// <summary>
    /// means "ContainSpLevelss" for DATATYPES and "HasSpLevels" for DATAITEMS (ADSDATATYPEFLAG_SPLEVELS)
    /// </summary>
    SpLevels = 262144, // 0x00040000
    /// <summary>
    /// do not restore persistent data (ADSDATATYPEFLAG_IGNOREPERSIST)
    /// </summary>
    IgnorePersist = 524288, // 0x00080000
    /// <summary>Any size array (ADSDATATYPEFLAG_ANYSIZEARRAY)</summary>
    /// <remarks>
    /// If the index is exeeded, a value access to this array will return <see cref="F:TwinCAT.Ads.AdsErrorCode.DeviceInvalidArrayIndex" />
    /// </remarks>
    AnySizeArray = 1048576, // 0x00100000
    /// <summary>
    ///  data type used for persistent variables -&gt; should be saved with persistent data (ADSDATATYPEFLAG_PERSIST_DT,0x00200000)
    /// </summary>
    PersistantDatatype = 2097152, // 0x00200000
    /// <summary>
    /// Persistent data will not restored after reset (cold) (ADSDATATYPEFLAG_INITONRESET,0x00400000)
    /// </summary>
    InitOnResult = 4194304, // 0x00400000
    /// <summary>None / No Flag set</summary>
    None = 0,
  }
}