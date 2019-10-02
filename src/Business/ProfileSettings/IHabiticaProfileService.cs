using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Models;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public interface IHabiticaProfileService
    {
        Task<HabiticaProfileModel> GetAsync(string userId, string apiKey, CancellationToken cancellationToken);
    }
}
