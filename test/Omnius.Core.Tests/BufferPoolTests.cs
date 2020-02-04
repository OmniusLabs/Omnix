using System;
using Xunit;

namespace Omnius.Core
{
    public class BufferPoolTests
    {
        [Fact]
        public void RentAndDisposeTest()
        {
            var random = new Random();
            var bytesPool = BytesPool.Create();

            for (int i = 0; i < 32; i++)
            {
                int size = random.Next(1, 1024 * 1024 * 32);
                var buffer = bytesPool.Memory.Rent(size);

                Assert.True(buffer.Memory.Length >= size);

                buffer.Dispose();
            }
        }
    }
}
