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
        public void FromExpression_Should_CorrectlyYield_When_NoInvalidValuesAreAccessed()
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

            stringResult.Should().NotBeNull();
            stringResult.Value.Should().Be(stringLeftMaybe.Value + stringRightMaybe.Value);
            intResult.Should().NotBeNull();
            intResult.Value.Should().Be(intLeftMaybe.Value + intRightMaybe.Value);
        }

        [Fact]
        public void FromExpression_Should_ShortCircuit_When_NothingInstanceAccessed()
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

            stringResult.Should().Be(Maybe<string>.Nothing);
            intResult.Should().Be(Maybe<int>.Nothing);
        }

        [Fact]
        public void FromExpression_Should_YieldNothing_When_WhereConditionUnmeet()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(20);

            var stringResult = from stringVal in stringMaybe
                               where stringVal.StartsWith("o")
                               select stringVal;
            var intResult = from intVal in intMaybe
                            where intVal > 30
                            select intVal;

            stringResult.Should().Be(Maybe<string>.Nothing);
            intResult.Should().Be(Maybe<int>.Nothing);
        }

        [Fact]
        public void FromExpression_Should_CorrectlyYield_When_WhereConditionMeet()
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
