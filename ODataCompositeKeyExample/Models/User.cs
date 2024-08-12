using System.ComponentModel.DataAnnotations.Schema;

namespace ODataCompositeKeyExample.Models;

public class User : IEquatable<User>
{
    public required string Id { get; set; }

    public required string LastName { get; set; }

    public required string FirstName { get; set; }

    public virtual string FullName => $"{FirstName} {LastName}";

    public bool IsAdmin { get; set; }

    public string? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public bool ShowMyAssetsOnly { get; set; }

    public ISet<int>? MyAssets { get; set; }

    public bool Equals(User? other) => Id.Equals(other?.Id, StringComparison.InvariantCultureIgnoreCase);
}
