// <copyright file="MaybeUnwrappExtensionsTest.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class MaybeUnwrappExtensionsTest
    {
        [Fact]
        public void OrElse_Should_ReturnLazyValue_When_InvokedOnNothing()
        {
            var stringMaybe = Maybe<string>.Nothing;
            Func<string> alternativeString = () => "hello";
            var intMaybe = Maybe<int>.Nothing;
            Func<int> alternativeInt = () => 10;

            var stringResult = stringMaybe.OrElse(alternativeString);
            var intResult = intMaybe.OrElse(alternativeInt);

            stringResult.Should()
                .Be(alternativeString(), "the alternative result is returned");
            intResult.Should()
                .Be(alternativeInt(), "the alternative result is returned");
        }

        [Fact]
        public void OrElse_Should_ReturnAlternativeValue_When_InvokedOnNothing()
        {
            var stringMaybe = Maybe<string>.Nothing;
            var alternativeString = "hello";
            var intMaybe = Maybe<int>.Nothing;
            var alternativeInt = 10;

            var stringResult = stringMaybe.OrElse(alternativeString);
            var intResult = intMaybe.OrElse(alternativeInt);

            stringResult.Should()
                .Be(alternativeString, "the alternative result is returned");
            intResult.Should()
                .Be(alternativeInt, "the alternative result is returned");
        }

        [Fact]
        public void OrElse_Should_ReturnWrappedValue_When_InvokedOnValidWrapper()
        {
            var stringMaybe = Maybe.Lift("hello");
            var intMaybe = Maybe.Lift(10);

            var stringResult = stringMaybe.OrElse("alternative");
            var intResult = intMaybe.OrElse(20);

            stringResult.Should()
                .Be(stringMaybe.Value, "the wrapped value result is returned");
            intResult.Should()
                .Be(intMaybe.Value, "the wrapped value is returned");
        }
    }
}
