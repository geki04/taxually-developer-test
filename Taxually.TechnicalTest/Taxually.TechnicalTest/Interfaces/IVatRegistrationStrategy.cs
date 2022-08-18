using System.Threading.Tasks;

namespace Taxually.TechnicalTest.Interfaces
{
    /// <summary>
    /// Ideally these interfaces should have been placed in a dedicated Interfaces project
    /// </summary>
    public interface IVatRegistrationStrategy
    {
        Task CalculateVatAsync();
    }
}
