using Kcd.Domain;
using Kcd.Persistence.DatabaseContext;

namespace Kcd.Persistence.Repositories;

public class AvatarRepository : GenericRepository<Avatar>, IAvatarRepository
{
    public AvatarRepository(UserApplicationDatabaseContext context) : base(context)
    {
    }
}
