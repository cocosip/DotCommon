using DotCommon.DependencyInjection;
using Xunit;

namespace DotCommon.Test.DependencyInjection
{
    public class ObjectAccessorTest
    {
        [Fact]
        public void Get_Set_Test()
        {
            var objectAccessorTestClass = new ObjectAccessorTestClass(1, "zhangsan");

            IObjectAccessor<ObjectAccessorTestClass> objectAccessor = new ObjectAccessor<ObjectAccessorTestClass>(objectAccessorTestClass);
            Assert.Equal(objectAccessorTestClass, objectAccessor.Value);
        }
    }

    public class ObjectAccessorTestClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ObjectAccessorTestClass()
        {

        }

        public ObjectAccessorTestClass(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
