using ODataCompositeKeyExample.Data;
using ODataCompositeKeyExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataCompositeKeyExample.Controllers;

public class AssetsController(AdminDbContext dbContext) : ODataController
{
    [EnableQuery]
    [HttpGet]
    public async Task<IList<Asset>> Get() => await dbContext.Assets.ToListAsync();

    [EnableQuery]
    [HttpGet]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Get([FromODataUri] int key)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (key <= 0)
            return BadRequest();

        var asset = await dbContext.Assets.FindAsync(key);
        return asset == null ? NotFound() : Ok(asset);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Asset), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Asset>> Post([FromBody] Asset asset)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (asset == null)
            return BadRequest();

        await dbContext.Assets.AddAsync(asset);
        await dbContext.SaveChangesAsync();

        return Created(asset);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Asset>> Put([FromODataUri] int key, [FromBody] Asset asset)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (key <= 0)
            return BadRequest();

        if (asset == null)
            return BadRequest();

        var assetToBeUpdated = await dbContext.Assets.FindAsync(key);
        if (assetToBeUpdated == null)
            return NotFound();

        dbContext.Entry(asset).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Asset>> Patch([FromODataUri] int key, Delta<Asset> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (key <= 0)
            return BadRequest();

        if (delta == null)
            return BadRequest();

        var original = await dbContext.Assets.FindAsync(key);
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
    public async Task<ActionResult<string>> Delete([FromODataUri] int key)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (key <= 0)
            return BadRequest();

        var assetToBeDeleted = await dbContext.Assets.FindAsync(key);
        if (assetToBeDeleted == null)
            return NotFound();

        dbContext.Assets.Remove(assetToBeDeleted);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
