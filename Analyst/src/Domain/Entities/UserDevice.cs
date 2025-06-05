using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities;

public class UserDevice : Entity<Guid>, IAggregateRoot, IAuditableEntity
{
    public string DeviceToken { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public UserDevice()
    {
    }

    public UserDevice(Guid id, string deviceToken, string deviceId, Guid userId) : base(id)
    {
        DeviceToken = deviceToken;
        DeviceId = deviceId;
        UserId = userId;
    }


    public static UserDevice Create(string deviceToken, string deviceId, Guid userId)
    {
        return new UserDevice(Guid.NewGuid(), deviceToken, deviceId, userId);
    }

    public void Update(string deviceToken)
    {
        DeviceToken = deviceToken;
    }
}