namespace DotNetFunctional.Maybe
{
    using System;

    public static class MaybeExtensions
    {
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selectFn)
        {
            if (source.HasValue)
            {
                var result = selectFn(source.Value);
                return result != null ? Maybe.Lift(result) : default;
            }
            else
            {
                return default;
            }
        }

        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TResult>> selectFn) => source.HasValue ? selectFn(source.Value) : default;
    }
}
