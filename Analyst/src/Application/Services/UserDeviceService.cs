using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Devices;



namespace Application.Services;

public class UserDeviceService(IUnitOfWork unitOfWork) : IUserDeviceService
{
    public async Task<Result<UserDevice>> AddUserDeviceAsync(UserDevice userDevice, CancellationToken cancellationToken)
    {
        var newUserDevice = await unitOfWork.Repository<UserDevice, Guid>().AddAsync(userDevice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserDevice.IsFailure ? newUserDevice.Error : newUserDevice.Value;
    }

    public async Task<Result<UserDevice>> UpdateDeviceAsync(UserDevice userDevice, CancellationToken cancellationToken)
    {
        var newUserDevice = await unitOfWork.Repository<UserDevice, Guid>().SoftUpdateAsync(userDevice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newUserDevice.IsFailure ? newUserDevice.Error : newUserDevice.Value;
    }
    public async Task<Result<bool>> IsDeviceExistsAsync(string deviceId, CancellationToken cancellationToken)
    {
        var spec = new DeviceUniqueSpecification(deviceId);

        var userExistsResult = await unitOfWork.Repository<UserDevice, Guid>()
            .ExistsAsync(spec, cancellationToken);

        if (userExistsResult.IsFailure)
            return Result.Failure<bool>(userExistsResult.Error);

        return Result.Success(userExistsResult.Value);
    }

    public async Task<Result<UserDevice>> CreateOrUpdateUserDevice(UserDevice userDevice, CancellationToken cancellationToken)
    {
        var isDeviceExistsResult = await IsDeviceExistsAsync(userDevice.DeviceId, cancellationToken);

        if (isDeviceExistsResult.IsFailure)
            return isDeviceExistsResult.Error;

        if (!isDeviceExistsResult.Value)
        {
            return await AddUserDeviceAsync(userDevice, cancellationToken);
        }

        return await UpdateDeviceAsync(userDevice, cancellationToken);
    }
    
    public async Task<Result<UserDevice>> GetDeviceTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var spec = new LatestDeviceTokenByUserIdSpecification(userId);
        var user = await unitOfWork.Repository<UserDevice,Guid>()
            .FindOneAsync(spec, cancellationToken);
        
        return user.IsFailure ? Result.Failure<UserDevice>(DomainErrors.NotFound<UserDevice>()) 
            : Result.Success(user.Value);
    }
}





