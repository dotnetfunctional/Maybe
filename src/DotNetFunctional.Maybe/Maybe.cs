// <copyright file="Maybe.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    /// <summary>
    /// Companion class for <see cref="Maybe{T}"/> that provides static factory methods.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Lifts a value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        public static Maybe<T> Lift<T>(T value) => value != null ? new Maybe<T>(value) : Maybe<T>.Nothing;

        /// <summary>
        /// Lifts a nullable-value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        public static Maybe<T> Lift<T>(T? value)
            where T : struct
            => value.HasValue ? Lift(value.Value) : Maybe<T>.Nothing;
    }
}
