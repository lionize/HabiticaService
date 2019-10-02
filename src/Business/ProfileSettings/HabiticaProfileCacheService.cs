using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Cache.Distributed;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Models;
using TIKSN.Serialization;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public class HabiticaProfileCacheService : DistributedCacheDecoratorBase<HabiticaProfileModel>, IHabiticaProfileService
    {
        private readonly IHabiticaProfileService _habiticaProfileService;

        public HabiticaProfileCacheService(
            IHabiticaProfileService habiticaProfileService,
            IDistributedCache distributedCache,
            ISerializer<byte[]> serializer,
            IDeserializer<byte[]> deserializer,
            IOptions<DistributedCacheDecoratorOptions> genericOptions,
            IOptions<DistributedCacheDecoratorOptions<HabiticaProfileModel>> specificOptions) : base(distributedCache, serializer, deserializer, genericOptions, specificOptions)
        {
            _habiticaProfileService = habiticaProfileService ?? throw new ArgumentNullException(nameof(habiticaProfileService));
        }

        public Task<HabiticaProfileModel> GetAsync(string userId, string apiKey, CancellationToken cancellationToken)
        {
            return GetFromDistributedCacheAsync($"{nameof(HabiticaProfileService)}_{userId}", cancellationToken, () => _habiticaProfileService.GetAsync(userId, apiKey, cancellationToken));
        }
    }
}
