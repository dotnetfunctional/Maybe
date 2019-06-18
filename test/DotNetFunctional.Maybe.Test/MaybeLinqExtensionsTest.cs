// <copyright file="MaybeLinqExtensionsTest.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe.Test
{
    using FluentAssertions;
    using Xunit;

    public class MaybeLinqExtensionsTest
    {
        [Fact]
        public void Select_Should_YieldNothing_When_InvokedOnNothing()
        {
            var stringMaybe = Maybe<string>.Nothing;
            var intMaybe = Maybe<int>.Nothing;

            var stringResult = stringMaybe.Select(val => $"Source val: {val} ");
            var intResult = intMaybe.Select(val => val + 10);

            stringResult.Should()
                .Be(Maybe<string>.Nothing, "the projection wont be applied on nothing");
            intResult.Should()
                .Be(Maybe<int>.Nothing, "the projection wont be applied on nothing");
        }

        [Fact]
        public void Select_Should_YieldProjectedValue_When_InvokedOnValidWrapper()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(0);
            string SelectString(string val) => $"Source val: {val} ";
            int SelectInt(int val) => val + 10;

            var stringResult = stringMaybe.Select(SelectString);
            var intResult = intMaybe.Select(SelectInt);

            stringResult.Should()
                .Be(stringMaybe.Map(SelectString), "the value was projected");
            intResult.Should()
                .Be(intMaybe.Map(SelectInt), "the value was projected");
        }

        [Fact]
        public void Where_Should_YieldSource_When_PredicateFullfiled()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(20);

            var stringResult = stringMaybe.Where(val => val.StartsWith("h"));
            var intResult = intMaybe.Where(val => val > 10);

            stringResult.Should()
                .Be(stringMaybe, "the source wrapper is yielded");
            intResult.Should()
                .Be(intMaybe, "the source wrapper is yielded");
        }

        [Fact]
        public void Where_Should_YieldNothing_When_PredicateUnmeet()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(20);

            var stringResult = stringMaybe.Where(val => val.StartsWith("x"));
            var intResult = intMaybe.Where(val => val > 20);

            stringResult.Should()
                .Be(Maybe<string>.Nothing, "the predicate evaluated on the source yielded false");
            intResult.Should()
                .Be(Maybe<int>.Nothing, "the predicate evaluated on the source yielded false");
        }

        [Fact]
        public void Where_Should_YieldNothing_When_InvokedOnNothing()
        {
            var stringMaybe = Maybe<string>.Nothing;
            var intMaybe = Maybe<int>.Nothing;

            var stringResult = stringMaybe.Where(val => val.StartsWith("h"));
            var intResult = intMaybe.Where(val => val > 10);

            stringResult.Should()
                .Be(Maybe<string>.Nothing, "the predicate wont be applied on nothing");
            intResult.Should()
                .Be(Maybe<int>.Nothing, "the predicate wont be applied on nothing");
        }

        [Fact]
        public void QueryExpression_Should_CorrectlyYield_When_NoInvalidValuesAreAccessed()
        {
            var stringLeftMaybe = Maybe.Lift("he");
            var stringRightMaybe = Maybe.Lift("llo");
            var intLeftMaybe = Maybe.Lift(10);
            var intRightMaybe = Maybe.Lift(20);

            var stringResult = from left in stringLeftMaybe
                               from right in stringRightMaybe
                               select left + right;
            var intResult = from left in intLeftMaybe
                            from right in intRightMaybe
                            select left + right;

            stringResult.Should()
                .Be(Maybe.Lift(stringLeftMaybe.Value + stringRightMaybe.Value));
            intResult.Should()
                .Be(Maybe.Lift(intLeftMaybe.Value + intRightMaybe.Value));
        }

        [Fact]
        public void QueryExpression_Should_ShortCircuit_When_NothingInstanceAccessed()
        {
            var stringLeftMaybe = Maybe.Lift("he");
            var stringRightMaybe = Maybe<string>.Nothing;
            var intLeftMaybe = Maybe.Lift(10);
            var intRightMaybe = Maybe<int>.Nothing;

            var stringResult = from left in stringLeftMaybe
                               from right in stringRightMaybe
                               select left + right;
            var intResult = from left in intLeftMaybe
                            from right in intRightMaybe
                            select left + right;

            stringResult.Should()
                .Be(Maybe<string>.Nothing);
            intResult.Should()
                .Be(Maybe<int>.Nothing);
        }

        [Fact]
        public void QueryExpression_Should_YieldNothing_When_WhereConditionUnmeet()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(20);

            var stringResult = from stringVal in stringMaybe
                               where stringVal.StartsWith("o")
                               select stringVal;
            var intResult = from intVal in intMaybe
                            where intVal > 30
                            select intVal;

            stringResult.Should()
                .Be(Maybe<string>.Nothing);
            intResult.Should()
                .Be(Maybe<int>.Nothing);
        }

        [Fact]
        public void QueryExpression_Should_CorrectlyYield_When_WhereConditionMeet()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(20);

            var stringResult = from stringVal in stringMaybe
                               where stringVal.StartsWith(stringMaybe.Value)
                               select stringVal;
            var intResult = from intVal in intMaybe
                            where intVal >= intMaybe.Value
                            select intVal;

            stringResult.Should()
                .NotBe(Maybe<string>.Nothing, "the condition was meet and wrapped value was mapped")
                .And
                .NotBeSameAs(stringMaybe, "a new wrapped was created")
                .And
                .Be(stringMaybe, "the new wrapper has the same value");
            intMaybe.Should()
                .NotBe(Maybe<int>.Nothing, "condition was meet and wrapped value was mapped")
                .And
                .NotBeSameAs(intMaybe, "a new wrapped was created")
                .And
                .Be(intMaybe, "the new wrapper has the same value");
        }
    }
}
