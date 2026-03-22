using System.Collections.Generic;
using DotCommon.Collections.Generic;
using Xunit;

namespace DotCommon.Test.Collections.Generic
{
    public class CollectionUtilTest
    {
        [Fact]
        public void GroupByMaxCount_EmptyList_ShouldReturnEmptyList()
        {
            var source = new List<int>();
            var result = CollectionUtil.GroupByMaxCount(source, 3);
            Assert.Empty(result);
        }

        [Fact]
        public void GroupByMaxCount_SingleElement_ShouldReturnSingleGroup()
        {
            var source = new List<int> { 1 };
            var result = CollectionUtil.GroupByMaxCount(source, 3);

            Assert.Single(result);
            Assert.Single(result[0]);
            Assert.Equal(1, result[0][0]);
        }

        [Fact]
        public void GroupByMaxCount_LessThanMaxCount_ShouldReturnSingleGroup()
        {
            var source = new List<int> { 1, 2 };
            var result = CollectionUtil.GroupByMaxCount(source, 5);

            Assert.Single(result);
            Assert.Equal(2, result[0].Count);
        }

        [Fact]
        public void GroupByMaxCount_ExactlyMaxCount_ShouldReturnSingleGroup()
        {
            var source = new List<int> { 1, 2, 3 };
            var result = CollectionUtil.GroupByMaxCount(source, 3);

            Assert.Single(result);
            Assert.Equal(3, result[0].Count);
        }

        [Fact]
        public void GroupByMaxCount_MoreThanMaxCount_ShouldSplitIntoMultipleGroups()
        {
            var source = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            var result = CollectionUtil.GroupByMaxCount(source, 3);

            Assert.Equal(3, result.Count);
            Assert.Equal(3, result[0].Count);
            Assert.Equal(3, result[1].Count);
            Assert.Single(result[2]);
        }

        [Fact]
        public void GroupByMaxCount_WithKeySelector_Ascending_ShouldSortAndGroup()
        {
            var source = new List<int> { 5, 1, 3, 2, 4 };
            var result = CollectionUtil.GroupByMaxCount(source, 2, x => x, true);

            Assert.Equal(3, result.Count);
            Assert.Equal(new List<int> { 1, 2 }, result[0]);
            Assert.Equal(new List<int> { 3, 4 }, result[1]);
            Assert.Equal(new List<int> { 5 }, result[2]);
        }

        [Fact]
        public void GroupByMaxCount_WithKeySelector_Descending_ShouldSortAndGroup()
        {
            var source = new List<int> { 5, 1, 3, 2, 4 };
            var result = CollectionUtil.GroupByMaxCount(source, 2, x => x, false);

            Assert.Equal(3, result.Count);
            Assert.Equal(new List<int> { 5, 4 }, result[0]);
            Assert.Equal(new List<int> { 3, 2 }, result[1]);
            Assert.Equal(new List<int> { 1 }, result[2]);
        }

        [Fact]
        public void GroupByMaxCount_WithKeySelector_EmptyList_ShouldReturnEmptyList()
        {
            var source = new List<int>();
            var result = CollectionUtil.GroupByMaxCount(source, 3, x => x);

            Assert.Empty(result);
        }

        [Fact]
        public void GroupByMaxCount_WithComplexType_ShouldGroupCorrectly()
        {
            var source = new List<TestItem>
            {
                new TestItem { Id = 1, Name = "A" },
                new TestItem { Id = 2, Name = "B" },
                new TestItem { Id = 3, Name = "C" },
                new TestItem { Id = 4, Name = "D" },
            };

            var result = CollectionUtil.GroupByMaxCount(source, 2);

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].Count);
            Assert.Equal(2, result[1].Count);
        }

        [Fact]
        public void GroupByMaxCount_WithKeySelector_ComplexType_ShouldGroupCorrectly()
        {
            var source = new List<TestItem>
            {
                new TestItem { Id = 3, Name = "C" },
                new TestItem { Id = 1, Name = "A" },
                new TestItem { Id = 2, Name = "B" },
            };

            var result = CollectionUtil.GroupByMaxCount(source, 2, x => x.Id, true);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0][0].Id);
            Assert.Equal(2, result[0][1].Id);
            Assert.Equal(3, result[1][0].Id);
        }

        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
}