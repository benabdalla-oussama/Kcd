using Kcd.Common.Enums;
using Kcd.Domain;

namespace Kcd.Persistence.Repositories;

public interface IUserApplicationRepository : IGenericRepository<UserApplication>
{
    Task<IEnumerable<UserApplication>> GetApplicationsAsync(ApplicationStatus? status = null, CancellationToken cancellationToken = default);
    Task<UserApplication?> GetUserApplicationByEmail(string email, CancellationToken cancellationToken = default);
}
