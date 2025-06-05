using Domain.Entities;
using Domain.Shared;


namespace Application.Common.Interfaces;

public interface IUserDeviceService
{
    Task<Result<UserDevice>> AddUserDeviceAsync(UserDevice userDevice, CancellationToken cancellationToken);
    Task<Result<UserDevice>> UpdateDeviceAsync(UserDevice userDevice, CancellationToken cancellationToken);
    Task<Result<bool>> IsDeviceExistsAsync(string deviceId, CancellationToken cancellationToken);
    Task<Result<UserDevice>> CreateOrUpdateUserDevice(UserDevice userDevice, CancellationToken cancellationToken);
    Task<Result<UserDevice>> GetDeviceTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}