using Kcd.Application.Models;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;

namespace Kcd.Application.Interfaces;

public interface IUserApplicationService
{
    /// <summary>
    /// Applies a new user application.
    /// </summary>
    /// <param name="request">The user application request containing application details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="UserApplicationResponse"/> object.</returns>
    /// <exception cref="BadRequestException">Thrown when a user application with the same email already exists.</exception>
    Task<UserApplicationResponse> ApplyAsync(UserApplicationRequest request);

    /// <summary>
    /// Approves a user application by its identifier.
    /// </summary>
    /// <param name="applicationId">The identifier of the user application to approve.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the user application with the specified ID is not found.</exception>
    Task ApproveApplicationAsync(Guid applicationId);

    /// <summary>
    /// Rejects a user application by its identifier.
    /// </summary>
    /// <param name="applicationId">The identifier of the user application to reject.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the user application with the specified ID is not found.</exception>
    Task RejectApplicationAsync(Guid applicationId);

    /// <summary>
    /// Retrieves user applications optionally filtered by status.
    /// </summary>
    /// <param name="status">The status to filter applications by. If null, returns all applications.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="UserApplicationResponse"/> objects.</returns>
    Task<IEnumerable<UserApplicationResponse>> GetApplicationsAsync(ApplicationStatus? status = null);
}