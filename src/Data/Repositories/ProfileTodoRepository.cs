using System;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories
{
    public class ProfileTodoRepository : MongoRepository<ProfileTodoEntity, Guid>, IProfileTodoRepository
    {
        public ProfileTodoRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "ProfileTodos")
        {
        }
    }
}