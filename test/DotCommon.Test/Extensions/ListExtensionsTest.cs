using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DotCommon.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace DotCommon.Test.Extensions
{
    public class ListExtensionsTest
    {
        [Fact]
        public void Remove_Test()
        {
            var list1 = new List<int>();
            list1.Add(1);
            list1.Add(2);
            list1.Add(3);
            list1.Add(4);
            list1.Add(100);
            list1.Add(200);

            Assert.True(list1.Remove(x => x == 1));

            Assert.Equal(5, list1.Count);
            Assert.DoesNotContain(1, list1);
        }

        [Fact]
        public void EqualList_Test()
        {
            var l1 = new List<string>()
            {
                "1"
            };
            var l2 = new List<string>()
            {
                "1",
                "2"
            };
            var l3 = new List<string>()
            {
                "2",
                "1",
            };
            var l4 = new List<string>()
            {
                "2",
                "1",
            };
            Assert.False(l1.EqualList(l2));
            Assert.False(l2.EqualList(l3));
            Assert.True(l3.EqualList(l4));

            var o1 = new ListExtensionsClass1(1, "zhangsan");
            var o2 = new ListExtensionsClass1(1, "zhangsan");
            Assert.True(o1.Equals(o2));

            var l5 = new List<ListExtensionsClass1>()
            {
                new ListExtensionsClass1(1,"zhangsan"),
                new ListExtensionsClass1(2,"lisi")
            };
            var l6 = new List<ListExtensionsClass1>()
            {
                new ListExtensionsClass1(1,"zhangsan"),
                new ListExtensionsClass1(2,"lisi")
            };

            Assert.True(l5.EqualList(l6));

        }


        class ListExtensionsClass1 : IEquatable<ListExtensionsClass1>
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ListExtensionsClass1()
            {

            }

            public ListExtensionsClass1(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public bool Equals(ListExtensionsClass1 other)
            {
                return other != null && (Id == other.Id && Name == other.Name);
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }
                return obj is ListExtensionsClass1 && Equals((ListExtensionsClass1)obj);
            }
            public override int GetHashCode()
            {
                return (StringComparer.InvariantCulture.GetHashCode(Id) ^ StringComparer.InvariantCulture.GetHashCode(Name));
            }

            //public bool Equals(ListExtensionsClass1 other)
            //{
            //    var r = Id == other.Id && Name == other.Name;
            //    return r;
            //}
        }


    }
}
