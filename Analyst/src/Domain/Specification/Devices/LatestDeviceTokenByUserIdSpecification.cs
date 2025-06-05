using Domain.Entities;

namespace Domain.Specification.Devices;

public class LatestDeviceTokenByUserIdSpecification : Specification<UserDevice, Guid>
{
    public LatestDeviceTokenByUserIdSpecification(Guid userId)
        : base(device => device.UserId == userId)
    {
        AddOrderByDescending(device => device.CreatedOnUtc);
        AddSelector(device => device.DeviceToken);
    }
}