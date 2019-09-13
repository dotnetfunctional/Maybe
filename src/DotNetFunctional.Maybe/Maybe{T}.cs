// <copyright file="Maybe{T}.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A wrapper for an optional value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// Nothing value.
        /// </summary>
        /// <remarks>
        /// As <see cref="Maybe{T}"/> is a value type, the value of default(Maybe{T})
        /// is equivalent to the invocation of the default ctor. of it, meaning that the object created
        /// will have all its properties with default values.
        /// </remarks>
        public static readonly Maybe<T> Nothing = default;

        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
        /// </summary>
        /// <param name="value">The value to store in this.</param>
        internal Maybe(T value)
        {
            this.value = value;
            this.HasValue = true;
        }

        /// <summary>
        /// Gets the wrapped value. Can be accessed only if is really present, otherwise throws.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if a value is not present.</exception>
        public T Value
        {
            get
            {
                if (!this.HasValue)
                {
                    throw new InvalidOperationException("value is not present");
                }

                return this.value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Maybe{T}"/> object has a valid value of its underlying type.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Implementation of the equality operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if left and right pass the equality check; false otherwise.</returns>
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        /// <summary>
        /// Implementation of the inequality operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>True if left and right arent equal; false otherwise.</returns>
        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        /// <inheritdoc/>
        public override string ToString() => this.HasValue ? $"Maybe<{this.value.ToString()}>" : "Maybe<Nothing>";

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Maybe<T> mb && this.Equals(mb);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(this.value) * 397) ^ this.HasValue.GetHashCode();
            }
        }

        /// <inheritdoc/>
        public bool Equals(Maybe<T> other) => this.HasValue.Equals(other.HasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);

        /// <summary>
        /// Matches the wrapped value and maps it into a another unwrapped value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping.</typeparam>
        /// <param name="someFn">The mapping function to be used when the current instance has a value. Should not throw exceptions.</param>
        /// <param name="none">The value to return when the current instance has no value.</param>
        /// <returns>The unwrapped mapped value.</returns>
        public TResult Match<TResult>(Func<T, TResult> someFn, TResult none) => this.HasValue ? someFn(this.value) : none;

        /// <summary>
        /// Maps the wrapped value into another wrapped value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping.</typeparam>
        /// <param name="mapFn">The mapping function. Should not throw exceptions.</param>
        /// <returns>The wrapped mapped value.</returns>
        public Maybe<TResult> Map<TResult>(Func<T, TResult> mapFn) => this.Bind(val => Maybe.Lift(mapFn(val)));

        /// <summary>
        /// Performs a side-effect at once.
        /// </summary>
        /// <param name="somethingFn">The side-effect to run if this is something.</param>
        /// <param name="nothingFn">The side-effect to run if this is Nothing.</param>
        /// <returns>The same wrapper.</returns>
        public Maybe<T> Tap(Action<T> somethingFn = null, Action nothingFn = null)
        {
            if (this.HasValue)
            {
                somethingFn?.Invoke(this.Value);
            }
            else
            {
                nothingFn?.Invoke();
            }

            return this;
        }

        /// <summary>
        /// Binds the wrapped value.
        /// If no value is stored, <paramref name="bindFn"/> is not invoked and this method immediately returns <see cref="Maybe{T}.Nothing"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the binding.</typeparam>
        /// <param name="bindFn">The binding function. Should not throw exceptions.</param>
        /// <returns>The binded wrapped value.</returns>
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> bindFn) => this.HasValue ? bindFn(this.value) : Maybe<TResult>.Nothing;
    }
}
