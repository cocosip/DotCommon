using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using DotCommon.Reflecting;

namespace DotCommon.Test.Reflecting
{
    public class MemberInfoExtensionsTest
    {
        public class MemberInfoExtensionsClass1
        {
            [MemberInfoExtensions1]
            public string UserName { get; set; }

            public int Age { get; set; }
        }

        [MemberInfoExtensions2]
        public class MemberInfoExtensionsClass2 
        {

        }

        public class MemberInfoExtensionsClass3 : MemberInfoExtensionsClass2
        {

        }

        public class MemberInfoExtensionsClass4 : MemberInfoExtensionsClass1
        {

        }


        public class MemberInfoExtensions1Attribute : Attribute
        {
        }

        public class MemberInfoExtensions2Attribute : Attribute
        {
        }

        [Fact]
        public void GetSingleAttributeOrNull_Test()
        {
            var member1 = typeof(MemberInfoExtensionsClass1).GetMember("Member1").FirstOrDefault();
            Assert.Throws<ArgumentNullException>(() =>
            {
                var a1 = member1.GetSingleAttributeOrNull<MemberInfoExtensions1Attribute>();
            });

            var member2 = typeof(MemberInfoExtensionsClass1).GetMember("UserName").FirstOrDefault();
            var a2 = member2.GetSingleAttributeOrNull<MemberInfoExtensions1Attribute>();
            Assert.NotNull(a2);

            var member3 = typeof(MemberInfoExtensionsClass1).GetMember("Age").FirstOrDefault();
            var a3 = member3.GetSingleAttributeOrNull<MemberInfoExtensions1Attribute>();
            Assert.Equal(default, a3);
        }

        [Fact]
        public void GetSingleAttributeOfTypeOrBaseTypesOrNull_Test()
        {
            var attr1 = typeof(MemberInfoExtensionsClass2).GetSingleAttributeOfTypeOrBaseTypesOrNull<MemberInfoExtensions2Attribute>(true);
            Assert.NotNull(attr1);

            var attr2 = typeof(MemberInfoExtensionsClass3).GetSingleAttributeOfTypeOrBaseTypesOrNull<MemberInfoExtensions2Attribute>(true);
            Assert.NotNull(attr2);

            var attr3 = typeof(MemberInfoExtensionsClass4).GetSingleAttributeOfTypeOrBaseTypesOrNull<MemberInfoExtensions2Attribute>(true);
            Assert.Null(attr3);
        }



    }
}
