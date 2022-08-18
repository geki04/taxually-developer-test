using System.Text;
using System.Threading.Tasks;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    /// <summary>
    /// Responsible for registering a company for a VAT number using a CSV
    /// </summary>
    public class CsvBasedVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly string queueName;
        private readonly VatRegistrationRequest request;

        public CsvBasedVatRegistrationStrategy(VatRegistrationRequest request, string queueName)
        {
            this.request = request;
            this.queueName = queueName;
        }

        public async Task CalculateVatAsync()
        {
            // France requires an excel spreadsheet to be uploaded to register for a VAT number
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");

            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            var excelQueueClient = new TaxuallyQueueClient();

            // Queue file to be processed
            await excelQueueClient.EnqueueAsync(this.queueName, csv);
        }
    }
}
