using DotCommon.Reflecting;
using System.Collections.Generic;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class DictionaryExpressionTest
    {

        [Fact]
        public void ObjectToDictionary_Test()
        {
            var o = new TestDictionaryObject()
            {
                Id = 1,
                Age = null,
                Name = null,
                Score = 30.2M
            };

            var dict = DictionaryExpression.ObjectToDictionary<TestDictionaryObject>(o);

            Assert.Equal(1, dict["Id"]);
            Assert.Null(dict["Age"]);
            Assert.Null(dict["Name"]);
            Assert.Equal(30.2M, dict["Score"]);


        }

        [Fact]
        public void DictionaryToObject_Test()
        {
            var dict = new Dictionary<string, object>()
            {
                {"Id",100},
                {"Age", null},
                {"Name","Hello" },
                {"Score",22M }

            };

            var o = DictionaryExpression.DictionaryToObject<TestDictionaryObject>(dict);


            //int id1 = 3;
            //o.Score = (decimal)id1;

            Assert.Equal(100, o.Id);
            Assert.Null(o.Age);
            Assert.Equal("Hello", o.Name);
            Assert.Equal(22M, o.Score);


        }

    }
}
