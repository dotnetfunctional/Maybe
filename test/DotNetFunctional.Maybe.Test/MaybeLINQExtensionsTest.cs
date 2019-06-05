// <copyright file="MaybeLINQExtensionsTest.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe.Test
{
    using FluentAssertions;
    using Xunit;

    public class MaybeLINQExtensionsTest
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
    }
}
