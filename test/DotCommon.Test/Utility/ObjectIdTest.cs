using DotCommon.Utility;
using System;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ObjectIdTest
    {
        [Fact]
        public void GenerateNewStringIdTest()
        {
            var stringObjectId1 = ObjectId.GenerateNewStringId();
            Assert.Equal(24, stringObjectId1.Length);
            var objectId1 = ObjectId.Parse(stringObjectId1);
            Assert.True(objectId1.CreationTime < DateTime.Now.AddSeconds(10));
            Assert.False(objectId1.Equals("1"));
            object objectId1_1 = new ObjectId(stringObjectId1);
            Assert.True(objectId1.Equals(objectId1_1));


            var objectId2 = ObjectId.GenerateNewId();
            var objectId3 = ObjectId.GenerateNewId();
            Assert.True(objectId3.CompareTo(objectId2) > 0);
            Assert.True(objectId2.CompareTo(objectId3) < 0);
            Assert.True(objectId3 >= objectId2);
            Assert.True(objectId3 > objectId2);
            Assert.True(objectId2 < objectId3);
            Assert.True(objectId2 <= objectId3);
            Assert.True(objectId2 != objectId3);

            var objectId4 = new ObjectId(objectId2.ToString());

            Assert.True(objectId4.Equals(objectId2));
            Assert.Equal(0, objectId4.CompareTo(objectId2));
            Assert.True(objectId4.GetHashCode() == objectId2.GetHashCode());
            Assert.True(objectId2 <= objectId4);
            Assert.True(objectId2 >= objectId4);
            Assert.True(objectId2 == objectId4);

            var time1 = DateTime.Now;
            var objectId5 = ObjectId.GenerateNewId(time1);
            Assert.Equal(24, objectId5.ToString().Length);
            Assert.True(objectId5.Timestamp > 0);

            var objectId6 = new ObjectId(DateTime.Now, 10, 1, 1);
            Assert.Equal(24, objectId6.ToString().Length);
            Assert.Equal(10, objectId6.Machine);
            Assert.Equal(1, objectId6.Pid);
            Assert.Equal(1, objectId6.Increment);
            var objectId7 = ObjectId.Empty;
            Assert.Equal(default(ObjectId), objectId7);

            var millSeconds1 = ObjectId.ToMillisecondsSinceEpoch(DateTime.Now);
            Assert.True(millSeconds1 < DateTime.Now.AddSeconds(10).Ticks);

            var expectedTime1 = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            Assert.Equal(expectedTime1, ObjectId.ToUniversalTime(DateTime.MinValue));
            var expectedTime2 = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            Assert.Equal(expectedTime2, ObjectId.ToUniversalTime(DateTime.MaxValue));
            Assert.Throws<ArgumentNullException>(() =>
            {
                string a = null;
                new ObjectId(a);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                byte[] b = null;
                new ObjectId(b);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                ObjectId.Parse(null);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ObjectId.Parse("123");
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                ObjectId.ParseHexString(null);
            });
            Assert.Throws<Exception>(() =>
            {
                ObjectId.ParseHexString("12345");
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                ObjectId.ToHexString(null);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new ObjectId(1, 16777216, 1, 1);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new ObjectId(1, 1, 1, 16777216);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                ObjectId.Unpack(null, out int t1, out int m1, out short p1, out int inc);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var b1 = new byte[2] { 1, 2 };
                ObjectId.Unpack(b1, out int t1, out int m1, out short p1, out int inc);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ObjectId.Pack(1, 16777216, 1, 1);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ObjectId.Pack(1, 1, 1, -1);
            });
        }
    }
}
