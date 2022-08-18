using NUnit.Framework;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;
using Taxually.TechnicalTest.Services;

namespace Taxually.TechnicalTest.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for the VAT registration factory
    /// This file should also be in a dedicated test project
    /// </summary>
    [TestFixture]
    public class VatRegistrationStrategyFactoryTests
    {
        private VatRegistrationStrategyFactory target;

        [SetUp]
        public void Setup()
        {
            this.target = new VatRegistrationStrategyFactory();
        }

        /// <summary>
        /// Happy-day scenario to demonstrate our services are called and working as expected
        /// </summary>
        [Test]
        [TestCase(Country.GB, typeof(ApiBasedVatRegistrationStrategy))]
        [TestCase(Country.FR, typeof(CsvBasedVatRegistrationStrategy))]
        [TestCase(Country.DE, typeof(XmlBasedVatRegistrationStrategy))]
        public void Supported_Country_Should_Return_CorrectRegistrationStrategy(
            Country supportedCountry,
            Type correspondingStrategyType)
        {
            // ARRANGE
            var request = new VatRegistrationRequest
            {
                CompanyId = "666",
                CompanyName = "TestCompany",
                Country = supportedCountry
            };

            // ACT
            var resultStrategy = target.FindVatStrategy(request);

            // ASSERT
            Assert.That(resultStrategy, Is.Not.Null);
            Assert.AreEqual(correspondingStrategyType, resultStrategy.GetType());
        }

        /// <summary>
        /// Negative test to demonstrate an unsupported country VAT registration
        /// </summary>
        [Test]
        [TestCase(Country.HU)]
        [TestCase(Country.None)]
        public void NotSupported_Country_Should_Throw_Exception(Country notSupportedCountry)
        {
            // ARRANGE
            var request = new VatRegistrationRequest
            {
                CompanyId = "666",
                CompanyName = "TestCompany",
                Country = notSupportedCountry
            };

            // ACT & ASSERT
            var exception = Assert.Throws<NotSupportedException>(
                () => { target.FindVatStrategy(request); });

            Assert.That(exception, Is.Not.Null);
            Assert.AreEqual($"Not suppported Country for VAT registration: {request.Country}", exception.Message);
        }
    }
}
