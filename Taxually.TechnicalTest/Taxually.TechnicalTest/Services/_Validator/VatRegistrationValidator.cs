using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    /// <summary>
    /// Responsible for validating a VAT registration request.
    /// </summary>
    public class VatRegistrationValidator : IVatRegistrationValidator
    {
        public bool Validate(VatRegistrationRequest request)
        {
            if (request == null)
            {
                return false;
            }

            if (request.Country == Country.None)
            {
                return false;
            }

            // and so on...

            return true;
        }
    }
}
