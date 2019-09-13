// <copyright file="MaybeTest{T}.cs" company="DotNetFunctional">
// Copyright (c) DotNetFunctional. All rights reserved.
//
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace DotNetFunctional.Maybe.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public class MaybeTest
    {
        [Fact]
        public void Equals_Should_ReturnTrue_When_ComparingSameInstance()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift(default(int));

            var intEquals = intMaybe.Equals(intMaybe);
            var stringEquals = stringMaybe.Equals(stringMaybe);

            intEquals.Should().BeTrue();
            stringEquals.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_InvokedWithOtherWithSameValue()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift(default(int));

            var intEquals = intMaybe.Equals(Maybe.Lift(intMaybe.Value));
            var stringEquals = stringMaybe.Equals(Maybe.Lift(stringMaybe.Value));

            intEquals.Should().BeTrue();
            stringEquals.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_ComparingSameNothingInstance()
        {
            var stringNothing = Maybe<string>.Nothing;
            var intNothing = Maybe<int>.Nothing;

            var intEquals = intNothing.Equals(intNothing);
            var stringEquals = stringNothing.Equals(stringNothing);

            intEquals.Should().BeTrue();
            stringEquals.Should().BeTrue();
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_ComparingAgainstDifferentValue()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift(default(int));

            var intEquals = intMaybe.Equals(Maybe.Lift(intMaybe.Value + 1));
            var stringEquals = stringMaybe.Equals(Maybe.Lift(string.Concat(stringMaybe.Value, "bla")));

            intEquals.Should().BeFalse();
            stringEquals.Should().BeFalse();
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_ComparingDifferentTypes()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift(default(int));

            var equals = intMaybe.Equals(stringMaybe);

            equals.Should().BeFalse();
        }

        [Fact]
        public void Map_Should_ProjectValue_When_ValidValueWrapped()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift<int>(default);
            int MapInt(int val) => val + 1;
            string MapString(string val) => val + "bla";

            var intMap = intMaybe.Map(MapInt);
            var stringMap = stringMaybe.Map(MapString);

            intMap.Should().Be(Maybe.Lift(MapInt(intMaybe.Value)));
            stringMap.Should().Be(Maybe.Lift(MapString(stringMaybe.Value)));
        }

        [Fact]
        public void Map_Should_ReturnNothing_When_InvokerIsNothing()
        {
            var intMaybe = Maybe<int>.Nothing;
            var stringMaybe = Maybe<string>.Nothing;

            var intMap = intMaybe.Map(val => default(int));
            var stringMap = stringMaybe.Map(val => string.Empty);

            intMap.Should().Be(Maybe<int>.Nothing);
            stringMap.Should().Be(Maybe<string>.Nothing);
        }

        [Fact]
        public void Map_Should_ReturnNothing_When_MappedValueIsInvalid()
        {
            var stringMaybe = Maybe.Lift(string.Empty);

            var mapResult = stringMaybe.Map(val => default(string));

            mapResult.Should().Be(Maybe<string>.Nothing);
        }

        [Fact]
        public void Bind_Should_ReturnNothing_When_InvokerIsNothing()
        {
            var intMaybe = Maybe<int>.Nothing;
            var stringMaybe = Maybe<string>.Nothing;

            var intBind = intMaybe.Bind(val => Maybe.Lift(val + 1));
            var stringBind = stringMaybe.Bind(val => Maybe.Lift(val + "bla"));

            intBind.Should().Be(Maybe<int>.Nothing);
            stringBind.Should().Be(Maybe<string>.Nothing);
        }

        [Fact]
        public void Bind_Should_ProjectValue_When_ValidValueWrapped()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift<int>(default);
            Maybe<int> MapInt(int val) => Maybe.Lift(val + 1);
            Maybe<string> MapString(string val) => Maybe.Lift(val + "bla");

            var intBind = intMaybe.Bind(MapInt);
            var stringBind = stringMaybe.Bind(MapString);

            intBind.Should().Be(MapInt(intMaybe.Value));
            stringBind.Should().Be(MapString(stringMaybe.Value));
        }

        [Fact]
        public void Match_Should_MatchValue_When_ValidValueWrapped()
        {
            var stringMaybe = Maybe.Lift(string.Empty);
            var intMaybe = Maybe.Lift<int>(default);
            int MatchInt(int val) => val + 1;
            string MatchString(string val) => val + "bla";

            var intMatch = intMaybe.Match(MatchInt, default);
            var stringMatch = stringMaybe.Match(MatchString, string.Empty);

            intMatch.Should().Be(MatchInt(intMaybe.Value));
            stringMatch.Should().Be(MatchString(stringMaybe.Value));
        }

        [Fact]
        public void Match_Should_MatchNothing_When_InvokerIsNothing()
        {
            var stringMaybe = Maybe<string>.Nothing;
            var intMaybe = Maybe<int>.Nothing;

            var intMatch = intMaybe.Match(val => val, default);
            var stringMatch = stringMaybe.Match(val => val, string.Empty);

            intMatch.Should().Be(default);
            stringMatch.Should().Be(string.Empty);
        }

        [Fact]
        public void Tap_Should_RunNothingSideEffectAndNotSomethingSideEffect_When_OnNothing()
        {
            var test = "initial";
            string result = default;
            var sut = Maybe<string>.Nothing;

            sut.Tap(v => result = v, () => result = test);

            result.Should().Be(test);
        }

        [Fact]
        public void Tap_Should_RunSomethingSideEffectAndNotNothingSideEffect_When_OnSomething()
        {
            string result = default;
            var sut = Maybe.Lift("something");

            sut.Tap(val => result = val, () => result = string.Empty);

            result.Should().Be(sut.Value);
        }

        public class MaybeTestReferenceTypes
        {
            public static IEnumerable<object[]> GetNothings()
            {
                yield return new object[] { Maybe<string>.Nothing };
                yield return new object[] { Maybe<List<object>>.Nothing };
                yield return new object[] { Maybe<object>.Nothing };
            }

            [Theory]
            [MemberData(nameof(GetNothings))]
            public void NothingIsFlaggedAsEmpty<T>(Maybe<T> refNothing)
                where T : class
            {
                refNothing.HasValue.Should()
                    .BeFalse();
            }

            [Theory]
            [MemberData(nameof(GetNothings))]
            public void NothingValueAccessorThrowsIfAccessed<T>(Maybe<T> refNothing)
                where T : class
            {
                Func<T> act = () => refNothing.Value;

                act.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage("value is not present");
            }

            [Fact]
            public void CorrectlyLiftsValidValues()
            {
                var result = Maybe.Lift(string.Empty);

                result.Should().NotBeNull();
                result.HasValue.Should().BeTrue();
                result.Value.Should().Be(string.Empty);
            }

            [Fact]
            public void When_LiftingNull_Should_ReturnNothing()
            {
                var nullString = Maybe.Lift<string>(default);
                var nullObject = Maybe.Lift<object>(default);

                nullString.Should().Be(Maybe<string>.Nothing);
                nullObject.Should().Be(Maybe<object>.Nothing);
            }
        }

        public class MaybeTestValueTypes
        {
            public static IEnumerable<object[]> GetNothings()
            {
                yield return new object[] { Maybe<int>.Nothing };
                yield return new object[] { Maybe<bool>.Nothing };
                yield return new object[] { Maybe<DateTime>.Nothing };
                yield return new object[] { Maybe<DateTimeOffset>.Nothing };
            }

            [Theory]
            [MemberData(nameof(GetNothings))]
            public void NothingIsFlaggedAsEmpty<T>(Maybe<T> valNothing)
                where T : struct
            {
                valNothing.HasValue.Should()
                    .BeFalse();
            }

            [Theory]
            [MemberData(nameof(GetNothings))]
            public void NothingValueAccessorThrowsIfAccessed<T>(Maybe<T> valNothing)
                where T : struct
            {
                Func<T> act = () => valNothing.Value;

                act.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage("value is not present");
            }

            [Fact]
            public void CorrectlyLiftsValidValues()
            {
                var intMaybe = Maybe.Lift<int>(default);
                var dateMaybe = Maybe.Lift<DateTime>(default);

                intMaybe.Should().NotBeNull();
                intMaybe.HasValue.Should().BeTrue();
                intMaybe.Value.Should().Be(default);
                dateMaybe.Should().NotBeNull();
                dateMaybe.HasValue.Should().BeTrue();
                dateMaybe.Value.Should().Be(default);
            }
        }
    }
}
