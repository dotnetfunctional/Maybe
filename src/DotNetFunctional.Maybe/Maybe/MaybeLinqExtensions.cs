// <copyright file="MaybeLinqExtensions.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="Maybe{T}"/> that enable LINQ support.
    /// </summary>
    public static class MaybeLinqExtensions
    {
        /// <summary>
        /// Projects the wrapped value into a new one and wraps it.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <typeparam name="TResult">The type to project the value into.</typeparam>
        /// <param name="source">The wrapper.</param>
        /// <param name="project">A transform function to apply on the wrapped value.</param>
        /// <returns>The projection of the wrapped value in a wrapper.</returns>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> project)
            => source.Bind(val => Maybe.Lift(project(val)));

        /// <summary>
        /// Projects the wrapped value into a new wrapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <typeparam name="TResult">The type to project the value into.</typeparam>
        /// <param name="source">The wrapper.</param>
        /// <param name="project">A transform function to apply on the wrapped value.</param>
        /// <returns>The projection of the wrapped value.</returns>
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TResult>> project)
            => source.Bind(val => project(val));

        /// <summary>
        /// Projects the wrapped value into a new wrapper and invokes a result selector function on it.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <typeparam name="TIntermediate">The type to project the value into.</typeparam>
        /// <typeparam name="TResult">The type of the selected value.</typeparam>
        /// <param name="source">The wrapper.</param>
        /// <param name="project">A transform function to apply on the wrapped value.</param>
        /// <param name="select">A result selector function to apply on the projected value.</param>
        /// <returns>The selected value wrapped.</returns>
        public static Maybe<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TIntermediate>> project,
            Func<TSource, TIntermediate, TResult> select) => source.Bind(sourceVal => project(sourceVal).Select(interVal => select(sourceVal, interVal)));

        /// <summary>
        /// Evaluates a condition on the wrapped value.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <param name="source">The wrapper.</param>
        /// <param name="predicate">A function to test the wrapped value for a condition.</param>
        /// <returns>
        /// <paramref name="source"/> if <paramref name="predicate"/> yields true when evaluated on the wrapped value; <see cref="Maybe{TSource}.Nothing"/> otherwise.
        /// </returns>
        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> source, Func<TSource, bool> predicate)
            => source.Bind(val => predicate(val) ? source : Maybe<TSource>.Nothing);
    }
}
