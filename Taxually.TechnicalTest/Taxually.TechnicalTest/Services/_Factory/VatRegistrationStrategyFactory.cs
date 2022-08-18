using System;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Services;

namespace Taxually.TechnicalTest.Interfaces
{
    public class VatRegistrationStrategyFactory : IVatRegistrationStrategyFactory
    {
        public IVatRegistrationStrategy FindVatStrategy(VatRegistrationRequest request) => request.Country switch
        {
            Country.GB => new ApiBasedVatRegistrationStrategy(request, "https://api.uktax.gov.uk"),

            Country.FR => new CsvBasedVatRegistrationStrategy(request, "vat-registration-csv"),

            Country.DE => new XmlBasedVatRegistrationStrategy(request, "vat-registration-xml"),

            _ => throw new NotSupportedException(
                    $"Not suppported Country for VAT registration: {request.Country}"),
        };
    }
}
