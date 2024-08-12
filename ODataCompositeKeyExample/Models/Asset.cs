using System.ComponentModel.DataAnnotations.Schema;

namespace ODataCompositeKeyExample.Models;

public class Asset : IEquatable<Asset>
{
    public required int Id { get; set; }

    public string? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public bool Equals(Asset? other) => Id.Equals(other.Id);
}

public class AssetSme
{
    public required int AssetId { get; set; }

    public required string UserId { get; set; }

    [NotMapped]
    public virtual User? User { get; set; }
}
