using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMysql.Models;
using APIMysql.Data;

[Route("api/[controller]")]
[ApiController]
public class EstadoController : ControllerBase
{
    private readonly APIDbContext _context;
    public EstadoController(APIDbContext context)
    {
        _context = context;
    }

    // GET: api/Estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estado>>> GetEstado()
    {
        return await _context.Estado.ToListAsync();
    }

    // GET: api/Estado/5
    [HttpGet("{sigla}")]
    public async Task<ActionResult<Estado>> GetEstado(string sigla)
    {
        var estado = await _context.Estado.FindAsync(sigla);

        if (estado == null)
        {
            return NotFound();
        }

        return estado;
    }

    // PUT: api/Estado/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{sigla}")]
    public async Task<IActionResult> PutEstado(string? sigla, Estado estado)
    {
        var estadoSigla = await _context.Estado.FindAsync(sigla);
        if (estadoSigla == null)
        {
            return Problem(
                    title: "Alteração Inválida",
                    detail: "Não foi localizada a Sigla informada",
                    statusCode: StatusCodes.Status404NotFound
                );
        }

        if (sigla != estado.Sigla)
        {
            return Problem(
                    title: "Alteração de Chave Primária Inválida",
                    detail: "Não é possível alterar a Sigla, pois ela é a chave primária do Estado.",
                    statusCode: StatusCodes.Status400BadRequest);
        }

        _context.Entry(estado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EstadoExists(sigla))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Estado
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Estado>> PostEstado(Estado estado)
    {
        estado.Sigla = estado.Sigla.ToUpper().Trim();

        bool siglaExiste = await _context.Estado.AnyAsync(e => e.Sigla.ToUpper() == estado.Sigla);

        if (siglaExiste)
        {
            //HTTP 400 avisando do erro de duplicação
            return BadRequest($"A sigla '{estado.Sigla}' já está cadastrada.");
        }

        _context.Estado.Add(estado);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEstado", new { sigla = estado.Sigla }, estado);
    }

    // DELETE: api/Estado/5
    [HttpDelete("{sigla}")]
    public async Task<IActionResult> DeleteEstado(string? sigla)
    {
        var estado = await _context.Estado.FindAsync(sigla);
        if (estado == null)
        {
            return NotFound();
        }

        _context.Estado.Remove(estado);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EstadoExists(string? sigla)
    {
        return _context.Estado.Any(e => e.Sigla == sigla);
    }
}
