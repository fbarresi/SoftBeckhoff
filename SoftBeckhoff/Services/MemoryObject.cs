using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftBeckhoff.Services
{
    public class MemoryObject
    {
        private readonly Dictionary<uint, List<byte>> memoryBlocks = new Dictionary<uint, List<byte>>();

        public byte[] GetData(uint indexGroup, uint offset, uint lenght)
        {
            if (memoryBlocks.ContainsKey(indexGroup))
            {
                return memoryBlocks[indexGroup].Skip((int) offset).Take((int) lenght).ToArray();
            }
            throw new ArgumentOutOfRangeException($"Memory block nr {indexGroup} doesn't exists!");
        }

        public int Count(uint indexGroup)
        {
            if (memoryBlocks.ContainsKey(indexGroup))
            {
                return memoryBlocks[indexGroup].Count;
            }

            return 0;
        }

        public void AddData(uint indexGroup, byte[] data)
        {
            if (memoryBlocks.ContainsKey(indexGroup))
            {
                memoryBlocks[indexGroup].AddRange(data);
            }
            else
                memoryBlocks.Add(indexGroup, new List<byte>(data));
        }
        
        public void SetData(uint indexGroup, byte[] data)
        {
            if (memoryBlocks.ContainsKey(indexGroup))
            {
                memoryBlocks[indexGroup] = new List<byte>(data);
            }
            else
                memoryBlocks.Add(indexGroup, new List<byte>(data));
        }
        
        public void SetData(uint indexGroup, uint offset, byte[] data)
        {
            if (memoryBlocks.ContainsKey(indexGroup))
            {
                if(memoryBlocks[indexGroup].Count < offset + data.Length)
                    throw new ArgumentOutOfRangeException($"Memory block nr {indexGroup} should have at least {offset + data.Length} but had {memoryBlocks[indexGroup].Count}");
                for (int i = 0; i < data.Length; i++)
                {
                    memoryBlocks[indexGroup][(int) (offset + i)] = data[i];
                }
            }
            else
                throw new ArgumentOutOfRangeException($"Memory block nr {indexGroup} doesn't exists!");
        }
    }
}