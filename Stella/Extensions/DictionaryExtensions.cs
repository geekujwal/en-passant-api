// <copyright file="DictionaryExtension.cs" company="Veel">
// Copyright (c) Veel. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Stella.Extensions;

public static class DictionaryExtension
{
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        if (exists)
        {
            return val;
        }
        val = value;
        return value;
    }

    public static bool TryUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
        if (Unsafe.IsNullRef(ref val))
        {
            return false;
        }

        val = value;
        return true;
    }
}