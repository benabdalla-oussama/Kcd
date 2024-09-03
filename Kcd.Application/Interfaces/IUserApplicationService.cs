using Kcd.Application.Models;
using Kcd.Common.Enums;

namespace Kcd.Application.Interfaces;

public interface IUserApplicationService
{
    Task<UserApplicationResponse> ApplyAsync(UserApplicationRequest request);
    Task ApproveApplicationAsync(Guid applicationId);
    Task RejectApplicationAsync(Guid applicationId);
    Task<IEnumerable<UserApplicationResponse>> GetApplicationsAsync(ApplicationStatus? status = null);
}