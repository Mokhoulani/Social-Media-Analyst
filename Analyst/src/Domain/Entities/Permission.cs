using Domain.Primitives;

namespace Domain.Entities;

public class Permission : Entity<int>
{
    public string Name { get; init; } = string.Empty;
    public Permission(int id, string name): base(id)
    {
        Name = name;
    }
}
