using Domain.Primitives;

namespace Domain.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, "Registered");

    public Role(int id, string name)
        : base(id, name)
    {
    }

    public IEnumerable<Permission> Permissions { get; set; }

    public IEnumerable<User> Users { get; set; }
}
