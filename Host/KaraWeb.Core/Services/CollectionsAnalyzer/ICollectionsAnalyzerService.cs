using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Jobs;

namespace KaraWeb.Core.Services.CollectionsAnalyzer
{
    public interface ICollectionsAnalyzerService
    {
        Task<Job> StartCollectionAnalyzeAsync(Collection collection, CancellationToken cancellationToken);
    }
}
