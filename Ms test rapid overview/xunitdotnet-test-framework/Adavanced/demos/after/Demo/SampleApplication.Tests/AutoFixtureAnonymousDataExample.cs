﻿using Ploeh.AutoFixture;
using Xunit;


namespace SampleApplication.Tests
{
    public class AutoFixtureAnonymousDataExample
    {
        [Fact]                
        public void TestWithHandCreatedString()
        {
            var inputString = "hello";

            var sut = new StringUtils();

            var result = sut.MakeUpper(inputString);

            Assert.Equal("HELLO", result);
        }









        [Fact]
        public void TestWithAutogeneratedString()
        {
            var fixture = new Fixture();

            //  use fixture to create "anonymous" data
            var inputString = fixture.Create<string>();

            var sut = new StringUtils();

            var result = sut.MakeUpper(inputString);

            Assert.Equal(inputString.ToUpper(), result);
        }







        [Fact]
        public void TestWithAutogeneratedSeededString()
        {
            var fixture = new Fixture();

            var inputString = fixture.Create<string>("sarah");

            var sut = new StringUtils();

            var result = sut.MakeUpper(inputString);

            Assert.Equal(inputString.ToUpper(), result);
        }






        [Fact]
        public void TestWithAutogeneratedNumberAndString()
        {
            var fixture = new Fixture();

            var inputString = fixture.Create<string>();
            var inputNum = fixture.Create<int>();

            var sut = new StringUtils();

            var result = sut.AppendNumberToString(inputString, 
                                                                        inputNum);

            Assert.Equal(inputString + inputNum, result);
        }







        [Fact]
        public void AutogeneratingComplexTypes()
        {
            var fixture = new Fixture();

            var sut = fixture.Create<Person>();
        }











        [Fact]
        public void AutogeneratingSequences()
        {
            var fixture = new Fixture();

            var names = fixture.CreateMany<string>();
            var tenNames = fixture.CreateMany<string>(10);

            var ages = fixture.CreateMany<int>();
            var tenAges = fixture.CreateMany<int>(10);

            var tenPeople = fixture.CreateMany<Person>(10);
        }

        // AutoFixture cheat sheet http://bit.ly/psautofix
    }
}