using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lab6.IndexCompression;
using NUnit.Framework;

namespace InformationRetrieval.Tests
{
    [TestFixture]
    public class VariableByteNumberEncoderTests
    {
        private VariableByteNumberEncoder _target;

        [SetUp]
        public void SetUp()
        {
            _target = new VariableByteNumberEncoder();
        }

        [Test]
        public void Numbers_after_encoding_and_decoding_should_be_the_same()
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                var numbers = GenerateTestData().ToArray();

                //Act
                var bytes = _target.EncodeNumbers(numbers);
                var decodedNumbers = _target.DecodeNumbers(bytes);

                //Assert
                decodedNumbers.ShouldAllBeEquivalentTo(numbers,
                    $"Error occured on the following data: {string.Join(", ", numbers)}");
            }
        }

        public static IEnumerable<int> GenerateTestData()
        {
            var random = new Random();
            for (int i = 0; i < random.Next(1, 10); i++)
            {
                yield return random.Next(0, Int32.MaxValue);
            }
        }
    }
}
