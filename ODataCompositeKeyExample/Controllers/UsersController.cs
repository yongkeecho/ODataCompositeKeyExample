using ODataCompositeKeyExample.Data;
using ODataCompositeKeyExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataCompositeKeyExample.Controllers;

public class UsersController(AdminDbContext dbContext) : ODataController
{
    [EnableQuery]
    [HttpGet]
    public async Task<IList<User>> Get() => await dbContext.Users.ToListAsync();

    [EnableQuery]
    [HttpGet]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Get([FromODataUri] string key)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(User), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Post([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (user == null)
            return BadRequest();

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return Created(user);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Put([FromODataUri] string key, [FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        if (user == null)
            return BadRequest();

        var userToBeUpdated = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        if (userToBeUpdated == null)
            return NotFound();

        dbContext.Entry(user).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> Patch([FromODataUri] string key, Delta<User> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        if (delta == null)
            return BadRequest();

        var original = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
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
    public async Task<ActionResult<string>> Delete([FromODataUri] string key)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        var userToBeDeleted = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        if (userToBeDeleted == null)
            return NotFound();

        dbContext.Users.Remove(userToBeDeleted);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
