using System;
using System.Collections.Generic;
using DotCommon.Collections;
using Xunit;

namespace DotCommon.Test.Collections
{
    public class TypeListTest
    {
        [Fact]
        public void Constructor_ShouldCreateEmptyList()
        {
            var typeList = new TypeList();
            Assert.Empty(typeList);
            Assert.False(typeList.IsReadOnly);
        }

        [Fact]
        public void Add_Generic_ShouldAddType()
        {
            var typeList = new TypeList();
            typeList.Add<string>();

            Assert.Single(typeList);
            Assert.Contains(typeof(string), typeList);
        }

        [Fact]
        public void Add_Type_ShouldAddValidType()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));

            Assert.Single(typeList);
            Assert.Contains(typeof(string), typeList);
        }

        [Fact]
        public void Add_Type_ShouldThrowForInvalidType()
        {
            var typeList = new TypeList<IComparable>();
            Assert.Throws<ArgumentException>(() => typeList.Add(typeof(object)));
        }

        [Fact]
        public void TryAdd_ShouldReturnTrueWhenAdded()
        {
            var typeList = new TypeList();
            var result = typeList.TryAdd<string>();

            Assert.True(result);
            Assert.Single(typeList);
        }

        [Fact]
        public void TryAdd_ShouldReturnFalseWhenAlreadyExists()
        {
            var typeList = new TypeList();
            typeList.Add<string>();
            var result = typeList.TryAdd<string>();

            Assert.False(result);
            Assert.Single(typeList);
        }

        [Fact]
        public void Contains_Generic_ShouldReturnCorrectResult()
        {
            var typeList = new TypeList();
            typeList.Add<string>();

            Assert.True(typeList.Contains<string>());
            Assert.False(typeList.Contains<int>());
        }

        [Fact]
        public void Contains_Type_ShouldReturnCorrectResult()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));

            Assert.Contains(typeof(string), typeList);
            Assert.DoesNotContain(typeof(int), typeList);
        }

        [Fact]
        public void Remove_Generic_ShouldRemoveType()
        {
            var typeList = new TypeList();
            typeList.Add<string>();
            typeList.Add<int>();

            typeList.Remove<string>();

            Assert.Single(typeList);
            Assert.DoesNotContain(typeof(string), typeList);
        }

        [Fact]
        public void Remove_Type_ShouldReturnTrueWhenRemoved()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));

            var result = typeList.Remove(typeof(string));

            Assert.True(result);
            Assert.Empty(typeList);
        }

        [Fact]
        public void Remove_Type_ShouldReturnFalseWhenNotFound()
        {
            var typeList = new TypeList();

            var result = typeList.Remove(typeof(string));

            Assert.False(result);
        }

        [Fact]
        public void Indexer_ShouldGetAndSetType()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));

            Assert.Equal(typeof(string), typeList[0]);

            typeList[0] = typeof(int);
            Assert.Equal(typeof(int), typeList[0]);
        }

        [Fact]
        public void Indexer_ShouldValidateTypeOnSet()
        {
            var typeList = new TypeList<IComparable>();
            typeList.Add(typeof(int));

            Assert.Throws<ArgumentException>(() => typeList[0] = typeof(object));
        }

        [Fact]
        public void Insert_ShouldInsertAtCorrectPosition()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Insert(0, typeof(int));

            Assert.Equal(typeof(int), typeList[0]);
            Assert.Equal(typeof(string), typeList[1]);
        }

        [Fact]
        public void Insert_ShouldValidateType()
        {
            var typeList = new TypeList<IComparable>();
            Assert.Throws<ArgumentException>(() => typeList.Insert(0, typeof(object)));
        }

        [Fact]
        public void IndexOf_ShouldReturnCorrectIndex()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Add(typeof(int));

            Assert.Equal(0, typeList.IndexOf(typeof(string)));
            Assert.Equal(1, typeList.IndexOf(typeof(int)));
            Assert.Equal(-1, typeList.IndexOf(typeof(decimal)));
        }

        [Fact]
        public void RemoveAt_ShouldRemoveAtCorrectPosition()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Add(typeof(int));
            typeList.RemoveAt(0);

            Assert.Single(typeList);
            Assert.Equal(typeof(int), typeList[0]);
        }

        [Fact]
        public void Clear_ShouldRemoveAllTypes()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Add(typeof(int));
            typeList.Clear();

            Assert.Empty(typeList);
        }

        [Fact]
        public void CopyTo_ShouldCopyToArray()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Add(typeof(int));

            var array = new Type[3];
            typeList.CopyTo(array, 1);

            Assert.Null(array[0]);
            Assert.Equal(typeof(string), array[1]);
            Assert.Equal(typeof(int), array[2]);
        }

        [Fact]
        public void GetEnumerator_ShouldEnumerateAllTypes()
        {
            var typeList = new TypeList();
            typeList.Add(typeof(string));
            typeList.Add(typeof(int));

            var types = new List<Type>();
            foreach (var type in typeList)
            {
                types.Add(type);
            }

            Assert.Equal(2, types.Count);
            Assert.Contains(typeof(string), types);
            Assert.Contains(typeof(int), types);
        }

        [Fact]
        public void TypeList_WithBaseType_ShouldRestrictTypes()
        {
            var typeList = new TypeList<IComparable>();
            typeList.Add(typeof(int));
            typeList.Add(typeof(string));

            Assert.Equal(2, typeList.Count);
        }

        [Fact]
        public void TypeList_WithBaseType_ShouldRejectNonAssignableType()
        {
            var typeList = new TypeList<IComparable>();

            Assert.Throws<ArgumentException>(() => typeList.Add(typeof(object)));
        }
    }
}