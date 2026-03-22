using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using DotCommon.Json.SystemTextJson.Modifiers;
using Xunit;

namespace DotCommon.Test.Json
{
    public class DotCommonIgnorePropertiesModifiersTest
    {
        [Fact]
        public void CreateModifyAction_ShouldReturnAction()
        {
            var modifier = new DotCommonIgnorePropertiesModifiers<TestModel, string>();
            var action = modifier.CreateModifyAction(x => x.Secret);

            Assert.NotNull(action);
        }

        [Fact]
        public void Modify_ShouldIgnoreProperty()
        {
            var modifier = new DotCommonIgnorePropertiesModifiers<TestModel, string>();
            var action = modifier.CreateModifyAction(x => x.Secret);

            var options = new JsonSerializerOptions();
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            
            var jsonTypeInfo = options.TypeInfoResolver.GetTypeInfo(typeof(TestModel), options);
            action(jsonTypeInfo);

            Assert.NotNull(jsonTypeInfo);
        }

        [Fact]
        public void Modify_WithDifferentType_ShouldNotModify()
        {
            var modifier = new DotCommonIgnorePropertiesModifiers<TestModel, string>();
            var action = modifier.CreateModifyAction(x => x.Secret);

            var options = new JsonSerializerOptions();
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            var jsonTypeInfo = options.TypeInfoResolver.GetTypeInfo(typeof(OtherModel), options);

            action(jsonTypeInfo);

            Assert.NotNull(jsonTypeInfo);
        }

        public class TestModel
        {
            public string Name { get; set; } = "";
            public string Secret { get; set; } = "";
        }

        public class OtherModel
        {
            public string Value { get; set; } = "";
        }
    }

    public class DotCommonIncludeNonPublicPropertiesModifiersTest
    {
        [Fact]
        public void CreateModifyAction_ShouldReturnAction()
        {
            var modifier = new DotCommonIncludeNonPublicPropertiesModifiers<TestModelWithPrivate, string>();
            var action = modifier.CreateModifyAction(x => x.Name);

            Assert.NotNull(action);
        }

        [Fact]
        public void Modify_ShouldIncludeNonPublicProperty()
        {
            var modifier = new DotCommonIncludeNonPublicPropertiesModifiers<TestModelWithPrivate, string>();
            var action = modifier.CreateModifyAction(x => x.Name);

            var options = new JsonSerializerOptions();
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            var jsonTypeInfo = options.TypeInfoResolver.GetTypeInfo(typeof(TestModelWithPrivate), options);

            action(jsonTypeInfo);

            Assert.NotNull(jsonTypeInfo);
        }

        [Fact]
        public void Modify_WithDifferentType_ShouldNotModify()
        {
            var modifier = new DotCommonIncludeNonPublicPropertiesModifiers<TestModelWithPrivate, string>();
            var action = modifier.CreateModifyAction(x => x.Name);

            var options = new JsonSerializerOptions();
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            var jsonTypeInfo = options.TypeInfoResolver.GetTypeInfo(typeof(OtherModel), options);

            action(jsonTypeInfo);

            Assert.NotNull(jsonTypeInfo);
        }

        public class TestModelWithPrivate
        {
            public string Name { get; private set; } = "";
        }

        public class OtherModel
        {
            public string Value { get; set; } = "";
        }
    }
}