using System.Threading;
using System.Threading.Tasks;

namespace ScioBlazor.Services
{
    public interface INameDeclensionService
    {
        Task<string?> GetInstrumentalFirstNameAsync(string firstName, CancellationToken cancellationToken = default);
    }
}
