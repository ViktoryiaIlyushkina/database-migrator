using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMigrator.EfCoreDefault.Data;
using TestMigrator.EfCoreDefault.Models;

namespace TestMigrator.EfCoreDefault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthorsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Author>> PostAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthors), new { id = author.Id }, author);
    }
}