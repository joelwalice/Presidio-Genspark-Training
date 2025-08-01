using MigrationProject.DTOs;
using MigrationProject.Interfaces;
using MigrationProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MigrationProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactUsController : ControllerBase
{
    private readonly IContactUsService _service;

    public ContactUsController(IContactUsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactUsReadDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactUsReadDto>> GetById(int id)
    {
        var contact = await _service.GetByIdAsync(id);
        return contact == null ? NotFound() : Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<ContactUsReadDto>> Create(ContactUsCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}