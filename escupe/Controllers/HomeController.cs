using Microsoft.AspNetCore.Mvc;
using escupe.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using escupe.Models;
using Microsoft.Extensions.Logging;
using escupe.ViewModels;


public class HomeController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IAuthService authService, ILogger<HomeController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EscolhaTipo()
    {
        // Renderiza a view EscolhaTipo.cshtml localizada em Views/Home
        return View("EscolhaTipo");
    }

    [HttpPost]
    public IActionResult EscolhaTipo(string TipoUsuario)
    {
        if (TipoUsuario == "C")
            return RedirectToAction("CadastrarCandidato", "Usuarios", new { tipoUsuario = "C" });
        else if (TipoUsuario == "E")
            return RedirectToAction("CadastrarEmpresa", "Usuarios", new { tipoUsuario = "E" });

        // Caso não selecionado, retorna para a tela de escolha
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        try
        {
            var usuarioAutenticado = await _authService.Authenticate(model.Email, model.Senha);

            if (usuarioAutenticado == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciais inválidas");
                return View("Index", model);
            }

            // Correção: Defina a lista de claims antes de usá-la
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, model.Email),
            new Claim("TipoUsuario", usuarioAutenticado.TipoUsuario)
        };

            if (usuarioAutenticado.TipoUsuario == "C")
            {
                var candidato = usuarioAutenticado.Usuario as Candidato;
                // Correção: int não pode ser null, então só verifica se é null o objeto
                if (candidato == null)
                {
                    ModelState.AddModelError(string.Empty, "Dados do candidato incompletos.");
                    return View("Index", model);
                }
                claims.Add(new Claim(ClaimTypes.NameIdentifier, candidato.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, "Candidato"));
            }
            else if (usuarioAutenticado.TipoUsuario == "E")
            {
                var empresa = usuarioAutenticado.Usuario as Empresa;
                if (empresa == null)
                {
                    ModelState.AddModelError(string.Empty, "Dados da empresa incompletos.");
                    return View("Index", model);
                }
                claims.Add(new Claim(ClaimTypes.NameIdentifier, empresa.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, "Empresa"));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tipo de usuário inválido.");
                return View("Index", model);
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redireciona para a View correta com o ViewModel adequado
            if (usuarioAutenticado.TipoUsuario == "C")
            {
                var feedVagaViewModel = new FeedVagaViewModel();
                // Carregue as vagas conforme sua lógica de negócio
                return RedirectToAction("FeedProcuraVaga", "Feed");
            }
            else // Empresa
            {
                var homeEmpresaViewModel = new HomeEmpresaViewModel();
                // Carregue os dados da empresa conforme sua lógica de negócio
                return RedirectToAction("HomeEmpresa", "Feed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante autenticação");
            TempData["ErrorMessage"] = "Ocorreu um erro durante o login. Por favor, tente novamente.";
            return RedirectToAction("Erro");
        }
    }
    public IActionResult Erro()
    {
        return View();
    }
}