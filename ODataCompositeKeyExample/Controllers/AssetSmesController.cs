using ODataCompositeKeyExample.Data;
using ODataCompositeKeyExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataCompositeKeyExample.Controllers;

public class AssetSmesController(AdminDbContext dbContext) : ODataController
{
    [EnableQuery]
    [HttpGet]
    public async Task<IList<AssetSme>> Get() => await dbContext.AssetSmes.ToListAsync();

    [EnableQuery]
    [HttpGet]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Get([FromODataUri] int keyAssetId, [FromODataUri] string keyUserId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (keyAssetId <= 0 || string.IsNullOrEmpty(keyUserId))
            return BadRequest();

        var assetSme = await dbContext.AssetSmes.FindAsync(keyAssetId, keyUserId);
        return assetSme == null ? NotFound() : Ok(assetSme);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AssetSme), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AssetSme>> Post([FromBody] AssetSme assetSme)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (assetSme == null)
            return BadRequest();

        await dbContext.AssetSmes.AddAsync(assetSme);
        await dbContext.SaveChangesAsync();

        return Created(assetSme);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AssetSme>> Put([FromODataUri] int keyAssetId, [FromODataUri] string keyUserId, [FromBody] AssetSme assetSme)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (keyAssetId <= 0 || string.IsNullOrEmpty(keyUserId))
            return BadRequest();

        if (assetSme == null)
            return BadRequest();

        var assetSmeToBeUpdated = await dbContext.AssetSmes.FindAsync(keyAssetId, keyUserId);
        if (assetSmeToBeUpdated == null)
            return NotFound();

        dbContext.Entry(assetSme).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AssetSme>> Patch([FromODataUri] int keyAssetId, [FromODataUri] string keyUserId, Delta<AssetSme> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (keyAssetId <= 0 || string.IsNullOrEmpty(keyUserId))
            return BadRequest();

        if (delta == null)
            return BadRequest();

        var original = await dbContext.AssetSmes.FindAsync(keyAssetId, keyUserId);
        if (original == null)
            return NotFound();

        delta.Patch(original);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<string>> Delete([FromODataUri] int keyAssetId, [FromODataUri] string keyUserId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (keyAssetId <= 0 || string.IsNullOrEmpty(keyUserId))
            return BadRequest();

        var assetSmeToBeDeleted = await dbContext.AssetSmes.FindAsync(keyAssetId, keyUserId);
        if (assetSmeToBeDeleted == null)
            return NotFound();

        dbContext.AssetSmes.Remove(assetSmeToBeDeleted);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
