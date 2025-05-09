﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace DotCommon.Json.SystemTextJson.Modifiers
{
    public class DotCommonIncludeNonPublicPropertiesModifiers<TClass, TProperty>
        where TClass : class
    {
        private Expression<Func<TClass, TProperty>> _propertySelector = default!;

        public Action<JsonTypeInfo> CreateModifyAction(Expression<Func<TClass, TProperty>> propertySelector)
        {
            _propertySelector = propertySelector;
            return Modify;
        }

        public void Modify(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Type == typeof(TClass))
            {
                var propertyName = _propertySelector.Body.As<MemberExpression>().Member.Name;
                var propertyJsonInfo = jsonTypeInfo.Properties.FirstOrDefault(x =>
                    x.AttributeProvider is MemberInfo memberInfo &&
                    memberInfo.Name == propertyName &&
                    x.Set == null);
                if (propertyJsonInfo != null)
                {
                    var propertyInfo = typeof(TClass).GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        propertyJsonInfo.Set = propertyInfo.SetValue;
                    }
                }
            }
        }
    }
}
