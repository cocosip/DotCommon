﻿using System;

namespace DotCommon
{
    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    [Serializable]
    public class NameValue : NameValue<string>
    {
        public NameValue()
        {

        }

        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// </summary>
    [Serializable]
    public class NameValue<T>
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Value.
        /// </summary>
        public T Value { get; set; } = default!;

        public NameValue()
        {

        }

        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
