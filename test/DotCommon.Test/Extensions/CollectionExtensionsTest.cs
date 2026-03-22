using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class CollectionExtensionsTest
    {
        [Fact]
        public void IsNullOrEmpty_WithNullCollection_ShouldReturnTrue()
        {
            ICollection<int> collection = null;
            Assert.True(collection.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_WithEmptyCollection_ShouldReturnTrue()
        {
            ICollection<int> collection = new List<int>();
            Assert.True(collection.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_WithItems_ShouldReturnFalse()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };
            Assert.False(collection.IsNullOrEmpty());
        }

        [Fact]
        public void AddIfNotContains_WithNewItem_ShouldAddAndReturnTrue()
        {
            ICollection<int> collection = new List<int>();

            var result = collection.AddIfNotContains(1);

            Assert.True(result);
            Assert.Single(collection);
            Assert.Equal(1, collection.First());
        }

        [Fact]
        public void AddIfNotContains_WithExistingItem_ShouldNotAddAndReturnFalse()
        {
            ICollection<int> collection = new List<int> { 1 };

            var result = collection.AddIfNotContains(1);

            Assert.False(result);
            Assert.Single(collection);
        }

        [Fact]
        public void AddIfNotContains_WithNullCollection_ShouldThrowArgumentNullException()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentNullException>(() => collection.AddIfNotContains(1));
        }

        [Fact]
        public void AddIfNotContains_WithMultipleItems_ShouldAddOnlyMissingItems()
        {
            ICollection<int> collection = new List<int> { 1, 2 };
            var itemsToAdd = new List<int> { 2, 3, 4 };

            var addedItems = collection.AddIfNotContains(itemsToAdd);

            Assert.Equal(4, collection.Count);
            Assert.Equal(2, addedItems.Count());
            Assert.Contains(3, addedItems);
            Assert.Contains(4, addedItems);
        }

        [Fact]
        public void AddIfNotContains_WithAllExistingItems_ShouldNotAddAny()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };
            var itemsToAdd = new List<int> { 1, 2, 3 };

            var addedItems = collection.AddIfNotContains(itemsToAdd);

            Assert.Equal(3, collection.Count);
            Assert.Empty(addedItems);
        }

        [Fact]
        public void AddIfNotContains_WithEmptyItemsToAdd_ShouldNotAddAny()
        {
            ICollection<int> collection = new List<int> { 1, 2 };
            var itemsToAdd = new List<int>();

            var addedItems = collection.AddIfNotContains(itemsToAdd);

            Assert.Equal(2, collection.Count);
            Assert.Empty(addedItems);
        }

        [Fact]
        public void AddIfNotContains_WithPredicateAndFactory_ShouldAddIfNotExists()
        {
            ICollection<string> collection = new List<string> { "existing" };

            var result = collection.AddIfNotContains(x => x == "new", () => "new");

            Assert.True(result);
            Assert.Equal(2, collection.Count);
            Assert.Contains("new", collection);
        }

        [Fact]
        public void AddIfNotContains_WithPredicateAndFactory_WhenExists_ShouldNotAdd()
        {
            ICollection<string> collection = new List<string> { "existing" };

            var result = collection.AddIfNotContains(x => x == "existing", () => "existing");

            Assert.False(result);
            Assert.Single(collection);
        }

        [Fact]
        public void AddIfNotContains_WithNullCollectionAndPredicate_ShouldThrowArgumentNullException()
        {
            ICollection<int> collection = null;
            Assert.Throws<ArgumentNullException>(() => collection.AddIfNotContains(x => x == 1, () => 1));
        }

        [Fact]
        public void AddIfNotContains_WithNullPredicate_ShouldThrowArgumentNullException()
        {
            ICollection<int> collection = new List<int>();
            Assert.Throws<ArgumentNullException>(() => collection.AddIfNotContains(null, () => 1));
        }

        [Fact]
        public void AddIfNotContains_WithNullFactory_ShouldThrowArgumentNullException()
        {
            ICollection<int> collection = new List<int>();
            Assert.Throws<ArgumentNullException>(() => collection.AddIfNotContains(x => x == 1, null));
        }

        [Fact]
        public void RemoveAll_WithPredicate_ShouldRemoveMatchingItems()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };

            var removed = collection.RemoveAll(x => x > 3);

            Assert.Equal(3, collection.Count);
            Assert.Equal(2, removed.Count);
            Assert.Contains(4, removed);
            Assert.Contains(5, removed);
        }

        [Fact]
        public void RemoveAll_WithPredicate_NoMatches_ShouldRemoveNothing()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };

            var removed = collection.RemoveAll(x => x > 10);

            Assert.Equal(3, collection.Count);
            Assert.Empty(removed);
        }

        [Fact]
        public void RemoveAll_WithPredicate_AllMatch_ShouldRemoveAll()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };

            var removed = collection.RemoveAll(x => x < 10);

            Assert.Empty(collection);
            Assert.Equal(3, removed.Count);
        }

        [Fact]
        public void RemoveAll_WithItems_ShouldRemoveSpecifiedItems()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
            var itemsToRemove = new List<int> { 2, 4 };

            collection.RemoveAll(itemsToRemove);

            Assert.Equal(3, collection.Count);
            Assert.DoesNotContain(2, collection);
            Assert.DoesNotContain(4, collection);
        }

        [Fact]
        public void RemoveAll_WithItems_NotInCollection_ShouldNotRemove()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };
            var itemsToRemove = new List<int> { 10, 20 };

            collection.RemoveAll(itemsToRemove);

            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void RemoveAll_WithEmptyItemsToRemove_ShouldNotRemoveAny()
        {
            ICollection<int> collection = new List<int> { 1, 2, 3 };
            var itemsToRemove = new List<int>();

            collection.RemoveAll(itemsToRemove);

            Assert.Equal(3, collection.Count);
        }
    }
}
