using System.Threading.Tasks;
using escupe.Models;

namespace escupe.Services
{
    
        public interface IAuthService
        {
        Task<UsuarioAutenticado> Authenticate(string email, string senha);

    }

}