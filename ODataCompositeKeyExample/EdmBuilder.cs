using ODataCompositeKeyExample.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataCompositeKeyExample;

/// <summary>
/// Edm Builder
/// </summary>
public static class EdmBuilder
{
    /// <summary>
    /// Gets EDM.
    /// </summary>
    /// <returns></returns>
    public static IEdmModel GetModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EnableLowerCamelCase();
        builder.EntitySet<User>("Users").EntityType.HasKey(user => user.Id);
        builder.EntitySet<Department>("Departments").EntityType.HasKey(department => department.Id);
        builder.EntitySet<Asset>("Assets").EntityType.HasKey(asset => asset.Id);
        builder.EntitySet<AssetSme>("AssetSMEs").EntityType.HasKey(sme => new { sme.AssetId, sme.UserId });
        return builder.GetEdmModel();
    }
}
