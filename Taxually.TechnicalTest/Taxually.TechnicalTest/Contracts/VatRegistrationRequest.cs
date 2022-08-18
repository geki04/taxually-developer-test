using Taxually.TechnicalTest.Interfaces;

namespace Taxually.TechnicalTest.Contracts
{
    /// <summary>
    /// Not sure whether this was intentional or part of the test, but this class should be in a separate file
    /// (or even better, a separate project)
    /// and the Country could have been stored as an enum to protect us against possible typos
    /// and a more dynamic way to extend the list of countries should we need to in the upcoming sprints.
    /// </summary>
    public class VatRegistrationRequest
    {
        public string CompanyName { get; set; }

        public string CompanyId { get; set; }

        public Country Country { get; set; }
    }
}
