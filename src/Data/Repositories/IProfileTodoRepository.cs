using System;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories
{
    public interface IProfileTodoRepository : IMongoRepository<ProfileTodoEntity, Guid>
    {
    }
}