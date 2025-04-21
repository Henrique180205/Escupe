using escupe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace escupe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            // Lógica de autenticação
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult CadastrarCandidato(Candidato model)
        {
            // Salvar no banco e redirecionar
            return RedirectToAction("Perfil");
        }

        [HttpPost]
        public IActionResult CadastrarEmpresa(Empresa model)
        {
            // Salvar no banco e redirecionar
            return RedirectToAction("PainelEmpresa");
        }
    }
