using ODataCompositeKeyExample.Data;
using ODataCompositeKeyExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataCompositeKeyExample.Controllers;

public class DepartmentsController(AdminDbContext dbContext) : ODataController
{
    [EnableQuery]
    [HttpGet]
    public async Task<IList<Department>> Get() => await dbContext.Departments.ToListAsync();

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

        var department = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        return department == null ? NotFound() : Ok(department);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Department), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Department>> Post([FromBody] Department department)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (department == null)
            return BadRequest();

        await dbContext.Departments.AddAsync(department);
        await dbContext.SaveChangesAsync();

        return Created(department);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Department>> Put([FromODataUri] string key, [FromBody] Department department)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        if (department == null)
            return BadRequest();

        var departmentToBeUpdated = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        if (departmentToBeUpdated == null)
            return NotFound();

        dbContext.Entry(department).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Department>> Patch([FromODataUri] string key, Delta<Department> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrEmpty(key))
            return BadRequest();

        if (delta == null)
            return BadRequest();

        var original = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
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

        var departmentToBeDeleted = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        if (departmentToBeDeleted == null)
            return NotFound();

        dbContext.Departments.Remove(departmentToBeDeleted);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
