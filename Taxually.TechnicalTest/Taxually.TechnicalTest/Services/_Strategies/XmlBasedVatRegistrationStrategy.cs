using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Contracts;
using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    /// <summary>
    /// Responsible for registering a company for a VAT number using an XML
    /// </summary>
    public class XmlBasedVatRegistrationStrategy : IVatRegistrationStrategy
    {
        private readonly string queueName;
        private readonly VatRegistrationRequest request;

        public XmlBasedVatRegistrationStrategy(VatRegistrationRequest request, string queueName)
        {
            this.request = request;
            this.queueName = queueName;
        }

        public async Task CalculateVatAsync()
        {
            // Germany requires an XML document to be uploaded to register for a VAT number
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringWriter, this.request);

                var xml = stringWriter.ToString();
                var xmlQueueClient = new TaxuallyQueueClient();

                // Queue xml doc to be processed
                await xmlQueueClient.EnqueueAsync(this.queueName, xml);
            }
        }
    }
}
