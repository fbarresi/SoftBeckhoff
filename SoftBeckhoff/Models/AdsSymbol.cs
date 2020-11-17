using System;
using System.Collections.Generic;
using System.Text;
using SoftBeckhoff.Enums;
using SoftBeckhoff.Extensions;
using SoftBeckhoff.Interfaces;
using SoftBeckhoff.Structs;

namespace SoftBeckhoff.Models
{
    public class AdsSymbol : IMarshable
    {
        private readonly string name;
        private readonly Type type;
        private AdsSymbolEntryHeader header;
        private string adsType;

        public AdsSymbol(string name, Type type)
        {
            this.name = name;
            this.type = type;
            header = new AdsSymbolEntryHeader(type);
            adsType = type.ToAdsDatatypeName();
        }

        public AdsSymbolEntryHeader Header => header;
        public string Name => name;
        public Type Type => type;
        public int Offset { get; set; }
        public int Size => (int) header.Size;

        public byte[] GetBytes()
        {
            var byteName = Encoding.ASCII.GetBytes(name);
            var byteType = Encoding.ASCII.GetBytes(adsType);
            var headerSize = header.GetSize();
            var wholeSize = headerSize + byteName.Length + byteType.Length + 3;
            header.NameLength = (ushort) byteName.Length;
            header.EntryLength = (uint) wholeSize;
            header.IndexOffset = (uint) Offset;
            var buffer = new List<byte>();
            buffer.AddRange(header.GetBytes());
            buffer.AddRange(byteName);
            buffer.Add(0);
            buffer.AddRange(byteType);
            buffer.Add(0);
            buffer.Add(0); //comment

            return buffer.ToArray();
        }
    }
}