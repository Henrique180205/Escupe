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

            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, model.Email),
            new Claim("TipoUsuario", usuarioAutenticado.TipoUsuario)
        };

            if (usuarioAutenticado.TipoUsuario == "C")
            {
                var candidato = (Candidato)usuarioAutenticado.Usuario;
                claims.Add(new Claim(ClaimTypes.NameIdentifier, candidato.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, "Candidato"));
            }
            else if (usuarioAutenticado.TipoUsuario == "E")
            {
                var empresa = (Empresa)usuarioAutenticado.Usuario;
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
                return View("~/Views/Feed/FeedProcuraVaga.cshtml", feedVagaViewModel);
            }
            else // Empresa
            {
                var homeEmpresaViewModel = new HomeEmpresaViewModel();
                // Carregue os dados da empresa conforme sua lógica de negócio
                return View("~/Views/Feed/HomeEmpresa.cshtml", homeEmpresaViewModel);
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