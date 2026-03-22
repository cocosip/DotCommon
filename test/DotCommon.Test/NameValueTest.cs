using System.Collections.Generic;
using DotCommon;
using Xunit;

namespace DotCommon.Test
{
    public class NameValueTest
    {
        [Fact]
        public void DefaultConstructor_ShouldCreateInstance()
        {
            var nameValue = new NameValue();
            Assert.NotNull(nameValue);
            Assert.Null(nameValue.Name);
            Assert.Null(nameValue.Value);
        }

        [Fact]
        public void Constructor_WithNameValue_ShouldSetProperties()
        {
            var nameValue = new NameValue("testName", "testValue");

            Assert.Equal("testName", nameValue.Name);
            Assert.Equal("testValue", nameValue.Value);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            var nameValue = new NameValue
            {
                Name = "newName",
                Value = "newValue"
            };

            Assert.Equal("newName", nameValue.Name);
            Assert.Equal("newValue", nameValue.Value);
        }
    }

    public class NameValueTTest
    {
        [Fact]
        public void DefaultConstructor_ShouldCreateInstance()
        {
            var nameValue = new NameValue<int>();
            Assert.NotNull(nameValue);
            Assert.Null(nameValue.Name);
            Assert.Equal(0, nameValue.Value);
        }

        [Fact]
        public void Constructor_WithNameValue_ShouldSetProperties()
        {
            var nameValue = new NameValue<int>("testName", 42);

            Assert.Equal("testName", nameValue.Name);
            Assert.Equal(42, nameValue.Value);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            var nameValue = new NameValue<List<int>>
            {
                Name = "list",
                Value = new List<int> { 1, 2, 3 }
            };

            Assert.Equal("list", nameValue.Name);
            Assert.Equal(new[] { 1, 2, 3 }, nameValue.Value);
        }

        [Fact]
        public void WithReferenceType_ShouldWork()
        {
            var nameValue = new NameValue<object>();
            nameValue.Name = "obj";
            nameValue.Value = new { Id = 1 };

            Assert.Equal("obj", nameValue.Name);
            Assert.NotNull(nameValue.Value);
        }
    }

    public class NameValueListTest
    {
        [Fact]
        public void List_ShouldStoreNameValueItems()
        {
            var list = new List<NameValue>
            {
                new NameValue("name1", "value1"),
                new NameValue("name2", "value2")
            };

            Assert.Equal(2, list.Count);
            Assert.Equal("name1", list[0].Name);
            Assert.Equal("value2", list[1].Value);
        }
    }
}