using DotCommon.Reflecting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class DictionaryEmitTest
    {
        [Fact]
        public void ObjectToDictionary_DictionaryToObject_Test()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                DictionaryEmit.DictionaryToObject<List<int>>(new Dictionary<string, string>());
            });


            Assert.Throws<NotSupportedException>(() =>
            {
                DictionaryEmit.GetDictionaryFunc<List<DictionaryEmitClass1>>();
            });

            var o = new DictionaryEmitClass1()
            {
                Id = 100,
                Name = "zhangsan",
                Age = 22,
                Birthday = new DateTime(2020, 3, 5)
            };

            var dict1 = DictionaryEmit.ObjectToDictionary<DictionaryEmitClass1>(o);
            Assert.Equal(o.Id.ToString(), dict1["Id"]);
            Assert.Equal(o.Name, dict1["Name"]);
            Assert.Equal(o.Age?.ToString(), dict1["Age"]);
            Assert.Equal(4, dict1.Count);


            var o1 = DictionaryEmit.DictionaryToObject<DictionaryEmitClass1>(dict1);
            Assert.Equal(o.Id, o1.Id);
            Assert.Equal(o.Name, o1.Name);
            Assert.Equal(o.Age, o1.Age);
            Assert.Equal(o.Birthday, o1.Birthday);

        }


        class DictionaryEmitClass1
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int? Age { get; set; }

            public decimal? Money { get; set; }

            public DateTime Birthday { get; set; }

            public DateTime? DeathDate { get; set; }

            private DictionaryEmitClass1State _state;
            public DictionaryEmitClass1State State
            {
                get
                {
                    if (_state == null)
                    {
                        _state = new DictionaryEmitClass1State(Id, Name);
                    }
                    return _state;
                }
            }

        }

        public class DictionaryEmitClass1State
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DictionaryEmitClass1State()
            {

            }

            public DictionaryEmitClass1State(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

    }
}
