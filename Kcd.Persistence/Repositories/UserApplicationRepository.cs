using Kcd.Common.Enums;
using Kcd.Domain;
using Kcd.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Kcd.Persistence.Repositories;

public class UserApplicationRepository : GenericRepository<UserApplication>, IUserApplicationRepository
{
    public UserApplicationRepository(UserApplicationDatabaseContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserApplication>> GetApplicationsAsync(ApplicationStatus? status = null, CancellationToken cancellationToken = default) =>
         await _context.UserApplications.Where(q => status == null || q.Status == status).OrderByDescending(x => x.DateCreated).ToListAsync(cancellationToken);

    public async Task<UserApplication?> GetUserApplicationByEmail(string email, CancellationToken cancellationToken = default) =>
         await _context.UserApplications.FirstOrDefaultAsync(q => q.Email == email, cancellationToken);
}
