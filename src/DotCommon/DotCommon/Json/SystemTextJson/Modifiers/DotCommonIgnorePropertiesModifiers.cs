﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace DotCommon.Json.SystemTextJson.Modifiers
{
    public class DotCommonIgnorePropertiesModifiers<TClass, TProperty>
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
                jsonTypeInfo.Properties.RemoveAll(
                    x => x.AttributeProvider is MemberInfo memberInfo &&
                         memberInfo.Name == _propertySelector.Body.As<MemberExpression>().Member.Name);
            }
        }
    }
}
