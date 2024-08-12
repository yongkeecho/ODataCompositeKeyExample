using System.ComponentModel.DataAnnotations.Schema;

namespace ODataCompositeKeyExample.Models;

public class Department : IEquatable<Department>
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<Asset>? Assets { get; set; }

    public virtual ICollection<User>? Users { get; set; }

    public bool Equals(Department? other) => Id.Equals(other?.Id, StringComparison.InvariantCultureIgnoreCase);
}
