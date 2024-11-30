using System.IO;
using Lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class DropdownController : ControllerBase
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "dropdowns.json");

    private List<Dropdown> LoadDropdowns()
    {
        if (System.IO.File.Exists(_filePath))
        {
            var json = System.IO.File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Dropdown>>(json);
        }
        return new List<Dropdown>();
    }

    private void SaveDropdowns(List<Dropdown> dropdowns)
    {
        var json = JsonConvert.SerializeObject(dropdowns, Formatting.Indented);
        System.IO.File.WriteAllText(_filePath, json);
    }

    // GET: Get all dropdowns
    [HttpGet(Name = "GetDropdowns")]
    public ActionResult<List<Dropdown>> Get()
    {
        var dropdowns = LoadDropdowns();
        return dropdowns;
    }

    // POST: Add a new dropdown
    [HttpPost]
    public ActionResult Add([FromBody] Dropdown dropdown)
    {
        if (dropdown == null)
        {
            return BadRequest("Dropdown cannot be null.");
        }

        var dropdowns = LoadDropdowns();

        // Generate a new ID
        dropdown.Id = dropdowns.Count > 0 ? dropdowns.Max(d => d.Id) + 1 : 1;
        dropdowns.Add(dropdown);

        SaveDropdowns(dropdowns);

        return CreatedAtAction(nameof(Get), new { id = dropdown.Id }, dropdown);
    }

    // PUT: Edit an existing dropdown
    [HttpPut("{id}")]
    public ActionResult Edit(int id, [FromBody] Dropdown updatedDropdown)
    {
        var dropdowns = LoadDropdowns();
        var existingDropdown = dropdowns.FirstOrDefault(d => d.Id == id);

        if (existingDropdown == null)
        {
            return NotFound($"Dropdown with ID {id} not found.");
        }

        existingDropdown.Name = updatedDropdown.Name;
        existingDropdown.Items = updatedDropdown.Items;

        SaveDropdowns(dropdowns);

        return NoContent();
    }

    // DELETE: Delete a dropdown by ID
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var dropdowns = LoadDropdowns();
        var dropdown = dropdowns.FirstOrDefault(d => d.Id == id);

        if (dropdown == null)
        {
            return NotFound($"Dropdown with ID {id} not found.");
        }

        dropdowns.Remove(dropdown);
        SaveDropdowns(dropdowns);

        return NoContent();
    }
}
