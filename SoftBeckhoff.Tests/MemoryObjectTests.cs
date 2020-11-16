using System;
using Shouldly;
using SoftBeckhoff.Services;
using Xunit;

namespace SoftBeckhoff.Tests
{
    public class MemoryObjectTests
    {
        [Fact]
        public void GetNotAllocatedMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            
            //assert
            Should.Throw(() => mo.GetData(1, 2, 3), typeof(ArgumentOutOfRangeException));
        }
        
        [Fact]
        public void GetMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            mo.SetData(1, new byte[1]);
            //assert
            mo.GetData(1, 0, 1).ShouldBe(new byte[1]);
        }
        
        [Fact]
        public void AddAndGetMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            mo.SetData(1, new byte[1]);
            mo.AddData(1,new byte[]{2});
            //assert
            mo.GetData(1, 1, 1).ShouldBe(new byte[]{2});
        }
        
        [Fact]
        public void OverrideMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            mo.SetData(1, new byte[4]);
            mo.SetData(1, 2, new byte[]{1,2});
            //assert
            mo.GetData(1, 2, 2).ShouldBe(new byte[] {1, 2});
            mo.GetData(1, 0, 4).ShouldBe(new byte[] {0, 0, 1, 2});
        }
        
        [Fact]
        public void ReadMoreThanAllocatedMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            mo.SetData(1, new byte[4]);
            //assert
            mo.GetData(1, 0, 40).ShouldBe(new byte[] {0, 0, 0, 0});
        }
        
        [Fact]
        public void CountNotAllocatedMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            //assert
            mo.Count(1).ShouldBe(0);
        }
        
        [Fact]
        public void CountAllocatedMemory()
        {
            //arrange
            var mo = new MemoryObject();
            
            //act
            mo.SetData(1, new byte[4]);
            //assert
            mo.Count(1).ShouldBe(4);
        }
    }
}