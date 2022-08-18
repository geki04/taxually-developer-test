using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatRegistrationValidator validator;
        private readonly IVatRegistrationStrategyFactory registrationFactory;

        /// <summary>
        /// Dependency injection using AutoMapper would be nice.
        /// </summary>
        public VatRegistrationController(
            [NotNull] IVatRegistrationValidator validator,
            [NotNull] IVatRegistrationStrategyFactory registrationFactory)
        {
            this.validator = validator;
            this.registrationFactory = registrationFactory;
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// If available, we might want to restrict who can access this endpoint by using attributes
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> VatRegistrationAsync([FromBody] VatRegistrationRequest request)
        {
            // Logging before returning with any status is also highly recommended
            // so that debugging with the help of Splunk is accelerated
            var validationResult = this.validator.Validate(request);
            if (!validationResult)
            {
                return BadRequest();
            }

            var selectedVatStrategy = this.registrationFactory.FindVatStrategy(request);
            await selectedVatStrategy.CalculateVatAsync();

            return Ok();
        }
    }
}
