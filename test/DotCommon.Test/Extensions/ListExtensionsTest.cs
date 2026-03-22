using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
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
        public void Remove_NotFound_ShouldReturnFalse()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.False(list.Remove(x => x == 999));
        }

        [Fact]
        public void EqualList_Test()
        {
            var l1 = new List<string>() { "1" };
            var l2 = new List<string>() { "1", "2" };
            var l3 = new List<string>() { "2", "1" };
            var l4 = new List<string>() { "2", "1" };

            Assert.False(l1.EqualList(l2));
            Assert.False(l2.EqualList(l3));
            Assert.True(l3.EqualList(l4));

            var l5 = new List<ListExtensionsClass1>()
            {
                new ListExtensionsClass1(1, "zhangsan"),
                new ListExtensionsClass1(2, "lisi")
            };
            var l6 = new List<ListExtensionsClass1>()
            {
                new ListExtensionsClass1(1, "zhangsan"),
                new ListExtensionsClass1(2, "lisi")
            };

            Assert.True(l5.EqualList(l6));
        }

        [Fact]
        public void EqualList_BothNull_ShouldReturnTrue()
        {
            List<string> l1 = null;
            List<string> l2 = null;
            Assert.True(l1.EqualList(l2));
        }

        [Fact]
        public void EqualList_OneNull_ShouldReturnFalse()
        {
            var l1 = new List<string> { "1" };
            List<string> l2 = null;
            Assert.False(l1.EqualList(l2));
            Assert.False(l2.EqualList(l1));
        }

        [Fact]
        public void EqualList_SameReference_ShouldReturnTrue()
        {
            var l1 = new List<string> { "1" };
            Assert.True(l1.EqualList(l1));
        }

        [Fact]
        public void InsertRange_ShouldInsertItems()
        {
            var list = new List<int> { 1, 4 };
            list.InsertRange(1, new[] { 2, 3 });

            Assert.Equal(new[] { 1, 2, 3, 4 }, list);
        }

        [Fact]
        public void FindIndex_ShouldReturnCorrectIndex()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            Assert.Equal(2, list.FindIndex(x => x == 3));
        }

        [Fact]
        public void FindIndex_NotFound_ShouldReturnMinusOne()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.Equal(-1, list.FindIndex(x => x == 999));
        }

        [Fact]
        public void AddFirst_ShouldAddAtBeginning()
        {
            var list = new List<int> { 2, 3 };
            list.AddFirst(1);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void AddLast_ShouldAddAtEnd()
        {
            var list = new List<int> { 1, 2 };
            list.AddLast(3);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertAfter_WithExistingItem_ShouldInsertAfter()
        {
            var list = new List<int> { 1, 3 };
            list.InsertAfter(1, 2);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertAfter_ItemNotFound_ShouldAddFirst()
        {
            var list = new List<int> { 2, 3 };
            list.InsertAfter(999, 1);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertAfter_WithPredicate_ShouldInsertAfter()
        {
            var list = new List<int> { 1, 3 };
            list.InsertAfter(x => x == 1, 2);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertBefore_WithExistingItem_ShouldInsertBefore()
        {
            var list = new List<int> { 1, 3 };
            list.InsertBefore(3, 2);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertBefore_ItemNotFound_ShouldAddLast()
        {
            var list = new List<int> { 1, 2 };
            list.InsertBefore(999, 3);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertBefore_WithPredicate_ShouldInsertBefore()
        {
            var list = new List<int> { 1, 3 };
            list.InsertBefore(x => x == 3, 2);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void ReplaceWhile_WithItem_ShouldReplaceMatchingItems()
        {
            var list = new List<int> { 1, 2, 3, 2, 4 };
            list.ReplaceWhile(x => x == 2, 99);
            Assert.Equal(new[] { 1, 99, 3, 99, 4 }, list);
        }

        [Fact]
        public void ReplaceWhile_WithFactory_ShouldReplaceMatchingItems()
        {
            var list = new List<int> { 1, 2, 3 };
            list.ReplaceWhile(x => x < 3, x => x * 10);
            Assert.Equal(new[] { 10, 20, 3 }, list);
        }

        [Fact]
        public void ReplaceOne_WithItem_ShouldReplaceFirstMatch()
        {
            var list = new List<int> { 1, 2, 2, 3 };
            list.ReplaceOne(x => x == 2, 99);
            Assert.Equal(new[] { 1, 99, 2, 3 }, list);
        }

        [Fact]
        public void ReplaceOne_WithFactory_ShouldReplaceFirstMatch()
        {
            var list = new List<int> { 1, 2, 3 };
            list.ReplaceOne(x => x == 2, x => x * 10);
            Assert.Equal(new[] { 1, 20, 3 }, list);
        }

        [Fact]
        public void ReplaceOne_WithComparer_ShouldReplace()
        {
            var list = new List<int> { 1, 2, 3 };
            list.ReplaceOne(2, 99);
            Assert.Equal(new[] { 1, 99, 3 }, list);
        }

        [Fact]
        public void MoveItem_ShouldMoveToTargetIndex()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            list.MoveItem(x => x == 3, 0);
            Assert.Equal(new[] { 3, 1, 2, 4, 5 }, list);
        }

        [Fact]
        public void MoveItem_SameIndex_ShouldNotChange()
        {
            var list = new List<int> { 1, 2, 3 };
            list.MoveItem(x => x == 2, 1);
            Assert.Equal(new[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void MoveItem_InvalidIndex_ShouldThrow()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.Throws<IndexOutOfRangeException>(() => list.MoveItem(x => x == 1, 10));
        }

        [Fact]
        public void GetOrAdd_ExistingItem_ShouldReturnExisting()
        {
            var list = new List<string> { "existing" };
            var result = list.GetOrAdd(x => x == "existing", () => "new");
            Assert.Equal("existing", result);
            Assert.Single(list);
        }

        [Fact]
        public void GetOrAdd_NewItem_ShouldAddAndReturn()
        {
            var list = new List<string>();
            var result = list.GetOrAdd(x => x == "new", () => "new");
            Assert.Equal("new", result);
            Assert.Single(list);
        }

        [Fact]
        public void SortByDependencies_ShouldSortCorrectly()
        {
            var items = new List<TestItem>
            {
                new TestItem { Name = "A", Dependencies = new List<string> { "B" } },
                new TestItem { Name = "B", Dependencies = new List<string>() },
                new TestItem { Name = "C", Dependencies = new List<string> { "A" } }
            };

            var sorted = ((IEnumerable<TestItem>)items).SortByDependencies(
                x => items.FindAll(i => x.Dependencies.Contains(i.Name)),
                new TestItemComparer()
            );

            Assert.Equal("B", sorted[0].Name);
            Assert.Equal("A", sorted[1].Name);
            Assert.Equal("C", sorted[2].Name);
        }

        private class TestItem
        {
            public string Name { get; set; }
            public List<string> Dependencies { get; set; } = new List<string>();
        }

        private class TestItemComparer : IEqualityComparer<TestItem>
        {
            public bool Equals(TestItem x, TestItem y) => x?.Name == y?.Name;
            public int GetHashCode(TestItem obj) => obj.Name?.GetHashCode() ?? 0;
        }

        class ListExtensionsClass1 : IEquatable<ListExtensionsClass1>
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public ListExtensionsClass1() { }

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
                if (obj is null) return false;
                return obj is ListExtensionsClass1 && Equals((ListExtensionsClass1)obj);
            }

            public override int GetHashCode()
            {
                return (StringComparer.InvariantCulture.GetHashCode(Id) ^ StringComparer.InvariantCulture.GetHashCode(Name));
            }
        }
    }
}
