namespace DotNetFunctional.Maybe
{
    using System;
    using System.Collections.Generic;

    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// Nothing value.
        /// </summary>
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
        /// Gets a value indicating whether a value is present or not.
        /// </summary>
        public bool HasValue { get; }

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        /// <inheritdoc/>
        public override string ToString() => this.HasValue ? $"<{this.value.ToString()}>" : "<Nothing>";

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

        public Maybe<TResult> Match<TResult>(Func<T, TResult> someFn, Func<T, TResult> noneFn)
        {
            if (this.HasValue)
            {
                var result = someFn(this.value);
                return result != null ? Maybe.Lift(result) : default;
            }
            else
            {
                return Maybe.Lift(noneFn(default));
            }
        }
    }
}
