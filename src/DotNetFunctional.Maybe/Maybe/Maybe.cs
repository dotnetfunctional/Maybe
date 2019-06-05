namespace DotNetFunctional.Maybe
{
    public static class Maybe
    {
        public static Maybe<T> Lift<T>(T value) => new Maybe<T>(value);
    }
}
