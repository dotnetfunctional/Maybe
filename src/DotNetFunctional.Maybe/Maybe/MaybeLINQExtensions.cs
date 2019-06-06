// <copyright file="MaybeLINQExtensions.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MaybeLINQExtensions
    {
        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> project)
            => source.Bind(val => Maybe.Lift(project(val)));

        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TResult>> project)
            => source.Bind(val => project(val));

        /// <summary>
        /// Enables LINQ support.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMiddle"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="project"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TMiddle>> project,
            Func<TSource, TMiddle, TResult> select) => source.Bind(sourceVal => project(sourceVal).Select(midVal => select(sourceVal, midVal)));

        /// <summary>
        /// Enables LINQ support
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> source, Func<TSource, bool> predicate)
            => source.Bind(val => predicate(val) ? source : Maybe<TSource>.Nothing);
    }
}
