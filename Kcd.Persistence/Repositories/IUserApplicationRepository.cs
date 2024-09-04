using Kcd.Common.Enums;
using Kcd.Domain;

namespace Kcd.Persistence.Repositories;

/// <summary>
/// Repository interface for managing user applications. Extends <see cref="IGenericRepository{UserApplication}"/>.
/// </summary>
public interface IUserApplicationRepository : IGenericRepository<UserApplication>
{
    /// <summary>
    /// Retrieves a collection of user applications with optional filtering by status.
    /// </summary>
    /// <param name="status">Optional status to filter the applications.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of a collection of <see cref="UserApplication"/>.</returns>
    Task<IEnumerable<UserApplication>> GetApplicationsAsync(ApplicationStatus? status = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user application by email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of the <see cref="UserApplication"/> if found, or <c>null</c> otherwise.</returns>
    Task<UserApplication?> GetUserApplicationByEmail(string email, CancellationToken cancellationToken = default);
}
