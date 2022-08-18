using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for the VAT registration controller
    /// This file should also be in a dedicated test project
    /// </summary>
    [TestFixture]
    public class VatRegistrationControllerTests
    {
        private Mock<IVatRegistrationValidator> mockValidator;
        private Mock<IVatRegistrationStrategy> mockRegistrationStrategy;
        private Mock<IVatRegistrationStrategyFactory> mockRegistrationFactory;

        private VatRegistrationController target;

        [SetUp]
        public void Setup()
        {
            this.mockValidator = new Mock<IVatRegistrationValidator>();
            this.mockRegistrationStrategy = new Mock<IVatRegistrationStrategy>();
            this.mockRegistrationFactory = new Mock<IVatRegistrationStrategyFactory>();

            this.mockValidator.Setup(
                mv => mv.Validate(It.IsAny<VatRegistrationRequest>())).Returns(true);

            this.mockRegistrationFactory.Setup(
                rf => rf.FindVatStrategy(It.IsAny<VatRegistrationRequest>())).Returns(this.mockRegistrationStrategy.Object);

            this.target = new VatRegistrationController(mockValidator.Object, mockRegistrationFactory.Object);
        }

        /// <summary>
        /// Happy-day scenario to demonstrate our services are called and working as expected
        /// </summary>
        [Test]
        public async Task Supported_Country_Should_Return_Ok()
        {
            // ARRANGE
            var request = new VatRegistrationRequest
            {
                CompanyId = "666",
                CompanyName = "TestCompany",
                Country = Country.GB
            };

            // ACT
            var result = await target.VatRegistrationAsync(request);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkResult>());

            this.mockValidator.Verify(mv => mv.Validate(request), Times.Once);
            this.mockValidator.VerifyNoOtherCalls();

            this.mockRegistrationStrategy.Verify(rs => rs.CalculateVatAsync(), Times.Once);
            this.mockRegistrationStrategy.VerifyNoOtherCalls();

            this.mockRegistrationFactory.Verify(mv => mv.FindVatStrategy(request), Times.Once);
            this.mockRegistrationFactory.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Negative test to demonstrate our a BadRequest
        /// </summary>
        [Test]
        public async Task Registratrion_With_InvalidRequest_Should_Return_BadRequest()
        {
            // ARRANGE
            var request = new VatRegistrationRequest
            {
                CompanyId = "666",
                CompanyName = "TestCompany",
                Country = Country.None
            };

            this.mockValidator.Setup(
                mv => mv.Validate(It.IsAny<VatRegistrationRequest>())).Returns(false);

            // ACT
            var result = await target.VatRegistrationAsync(request);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestResult>());

            this.mockValidator.Verify(mv => mv.Validate(request), Times.Once);
            this.mockValidator.VerifyNoOtherCalls();

            this.mockRegistrationStrategy.Verify(rs => rs.CalculateVatAsync(), Times.Never);
            this.mockRegistrationStrategy.VerifyNoOtherCalls();

            this.mockRegistrationFactory.Verify(mv => mv.FindVatStrategy(It.IsAny<VatRegistrationRequest>()), Times.Never);
            this.mockRegistrationFactory.VerifyNoOtherCalls();
        }
    }
}
