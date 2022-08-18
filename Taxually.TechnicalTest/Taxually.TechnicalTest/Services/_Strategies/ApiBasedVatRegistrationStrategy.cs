using System.Threading.Tasks;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    /// <summary>
    /// Responsible for registering a company for a VAT number using an API
    /// </summary>
    public class ApiBasedVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly string apiAddress;
        private readonly VatRegistrationRequest request;

        public ApiBasedVatRegistrationStrategy(VatRegistrationRequest request, string apiAddress)
        {
            this.request = request;
            this.apiAddress = apiAddress;
        }

        public async Task CalculateVatAsync()
        {
            // UK has an API to register for a VAT number
            var httpClient = new TaxuallyHttpClient();

            await httpClient.PostAsync(apiAddress, request);
        }
    }
}
