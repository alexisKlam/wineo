﻿using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class LinqExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        if (condition)
            return source.Where(predicate);
        else
            return source;
    }

    public static IEnumerable<TSource> WhereIf<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> predicate)
    {
        if (condition)
            return source.Where(predicate);
        else
            return source;
    }
}