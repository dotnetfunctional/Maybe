// <copyright file="MaybeLINQExtensions.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    using System;

    public static class MaybeLINQExtensions
    {
        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selectFn"></param>
        /// <returns></returns>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selectFn)
            => source.Bind(val => Maybe.Lift(selectFn(val)));

        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selectFn"></param>
        /// <returns></returns>
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TResult>> selectFn)
            => source.Bind(val => selectFn(val));

        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TOther"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selectFn"></param>
        /// <param name="projectFn"></param>
        /// <returns></returns>
        public static Maybe<TResult> SelectMany<TSource, TOther, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TOther>> selectFn,
            Func<TSource, TOther, TResult> projectFn) => source.Bind(sourceVal => selectFn(sourceVal).Select(otherVal => projectFn(sourceVal, otherVal)));
    }
}
