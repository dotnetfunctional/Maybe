// <copyright file="MaybeUnwrappExtensions.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    using System;

    /// <summary>
    /// Provides extension methods to <see cref="Maybe{T}"/> that enable unwrapp operations.
    /// </summary>
    public static class MaybeUnwrappExtensions
    {
        /// <summary>
        /// Returns an alternative value provided by a delegate function in case that the wrapper has none.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="else">A function that is invoked when the wrapper contains no value. The function must not throw.</param>
        /// <returns>The value wrapped by <paramref name="wrapper"/> if present; the result of invoking <paramref name="else"/> otherwise.</returns>
        public static TSource OrElse<TSource>(this Maybe<TSource> wrapper, Func<TSource> @else) =>
            wrapper.HasValue ? wrapper.Value : @else();

        /// <summary>
        /// Returns an alternative value in case that the wrapper has none.
        /// </summary>
        /// <typeparam name="TSource">The type of the wrapped value.</typeparam>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="else">The alternative value.</param>
        /// <returns>The value wrapped by <paramref name="wrapper"/> if present; <paramref name="else"/> otherwise.</returns>
        public static TSource OrElse<TSource>(this Maybe<TSource> wrapper, TSource @else) =>
            wrapper.HasValue ? wrapper.Value : @else;
    }
}
