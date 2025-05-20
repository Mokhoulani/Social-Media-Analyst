using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specification.Devices;

public class DeviceUniqueSpecification(string deviceId) : Specification<UserDevice, Guid>(d => d.DeviceId == deviceId);