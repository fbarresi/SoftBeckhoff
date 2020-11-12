namespace SoftBeckhoff.Structs
{
    public enum AdsSymbolFlags : ushort
    {
        /// <summary>None</summary>
        None = 0,
        /// <summary>ADSSYMBOLFLAG_PERSISTENT</summary>
        Persistent = 1,
        /// <summary>ADSSYMBOLFLAG_BITVALUE</summary>
        BitValue = 2,
        /// <summary>ADSSYMBOLFLAG_REFERENCETO</summary>
        ReferenceTo = 4,
        /// <summary>ADSSYMBOLFLAG_TYPEGUID</summary>
        TypeGuid = 8,
        /// <summary>ADSSYMBOLFLAG_TCCOMIFACEPTR</summary>
        TComInterfacePtr = 16, // 0x0010
        /// <summary>ADSSYMBOLFLAG_READONLY</summary>
        ReadOnly = 32, // 0x0020
        /// <summary>ADSSYMBOLFLAG_ITFMETHODACCESS</summary>
        ItfMethodAccess = 64, // 0x0040
        /// <summary>ADSSYMBOLFLAG_METHODDEREF</summary>
        MethodDeref = 128, // 0x0080
        /// <summary>ADSSYMBOLFLAG_CONTEXTMASK (4 Bit)</summary>
        ContextMask = 3840, // 0x0F00
        /// <summary>ADSSYMBOLFLAG_ATTRIBUTES</summary>
        Attributes = 4096, // 0x1000
        /// <summary>
        /// Symbol is static (ADSSYMBOLFLAG_STATIC,0x2000)
        /// </summary>
        Static = 8192, // 0x2000
        /// <summary>
        /// Persistent data will not restored after reset (cold, ADSSYMBOLFLAG_INITONRESET 0x4000)
        /// </summary>
        InitOnReset = 16384, // 0x4000
        /// <summary>
        /// Extended Flags in symbol (ADSSYMBOLFLAG_EXTENDEDFLAGS,0x8000)
        /// </summary>
        ExtendedFlags = 32768, // 0x8000
    }
}