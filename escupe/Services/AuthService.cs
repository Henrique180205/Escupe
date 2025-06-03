using escupe.Data;
using escupe.Models;
using escupe.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UsuarioAutenticado> Authenticate(string email, string senha)
    {
        var candidato = await _context.Candidato.FirstOrDefaultAsync(c => c.Email == email && c.Senha == senha);
        if (candidato != null)
            return new UsuarioAutenticado { Usuario = candidato, TipoUsuario = "C" };

        var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Email == email && e.Senha == senha);
        if (empresa != null)
            return new UsuarioAutenticado { Usuario = empresa, TipoUsuario = "E" };

        return null;
    }
}
