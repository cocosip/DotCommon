using System;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void GetAssembly_Test()
        {
            var assembly = typeof(TypeExtensionsTest).GetAssembly();
            Assert.Equal("DotCommon.Test", assembly.GetName().Name);
        }

        [Fact]
        public void GetMethod_Test()
        {
            var methodInfo = typeof(TypeExtensionsTest).GetMethod("Method1", 1, 0);
            Assert.Equal("Method1", methodInfo.Name);
        }


        public string Method1(int id)
        {
            return $"{id}-Name";
        }

    }
}
