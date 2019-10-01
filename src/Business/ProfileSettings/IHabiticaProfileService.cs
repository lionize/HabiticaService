using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public interface IHabiticaProfileService
    {
        Task<HabiticaProfileModel> GetAsync(string userId, string apiKey, CancellationToken cancellationToken);
    }
}
