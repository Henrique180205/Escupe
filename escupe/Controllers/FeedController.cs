using Microsoft.AspNetCore.Mvc;
using escupe.Data;
using escupe.Models;
using System.Linq;
using escupe.ViewModels;
using Microsoft.EntityFrameworkCore;

public class FeedController : Controller
{
    private readonly ApplicationDbContext _context;

    public FeedController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult PerfilCandidato()
    {
        var candidatoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (candidatoId == null)
            return RedirectToAction("Login", "Home");

        // Inclui o endereço relacionado
        var candidato = _context.Candidato
            .Include(c => c.Endereco)
            .FirstOrDefault(c => c.Id == int.Parse(candidatoId));
        if (candidato == null)
            return NotFound();

        return View(candidato);
    }

    [HttpGet]
    public IActionResult PerfilEmpresa()
    {
        var empresaId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (empresaId == null)
            return RedirectToAction("Login", "Home");

        var empresa = _context.Empresas.FirstOrDefault(e => e.Id == int.Parse(empresaId));
        if (empresa == null)
            return NotFound();

        return View(empresa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PerfilEmpresa(Empresa model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var empresaId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (empresaId == null)
            return RedirectToAction("Login", "Home");

        var empresa = _context.Empresas.FirstOrDefault(e => e.Id == int.Parse(empresaId));
        if (empresa == null)
            return NotFound();

        // Atualiza os campos editáveis
        empresa.NomeFantasia = model.NomeFantasia;
        empresa.CNPJ = model.CNPJ;
        empresa.AreaAtuacao = model.AreaAtuacao;
        empresa.Telefone = model.Telefone;
        empresa.Email = model.Email;
        empresa.Senha = model.Senha;

      

        _context.SaveChanges();

        ViewBag.MensagemSucesso = "Perfil editado com sucesso!";
        return View(empresa);
    }


    [HttpGet]
    public IActionResult HomeEmpresa()
    {
        return View();
    }


    [HttpGet]
    public IActionResult CriarVaga()
    {
        return View();
    }

    // POST: Feed/CriarVaga
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CriarVaga(string titulo, string local, string salario, string descricao, string beneficios,Vaga model)
    {

        ModelState.Remove(nameof(model.Empresa));

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        // Recupera o ID da empresa logada
        var empresaId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (empresaId == null)
            return RedirectToAction("Login", "Home");
        // Converte o salário de string para decimal
        decimal salarioDecimal = 0;
        decimal.TryParse(salario, out salarioDecimal);
        // Cria a vaga
        var vaga = new Vaga
        {
            Titulo = titulo,
            Localizacao = local,
            Salario = salarioDecimal,
            Descricao = descricao,
            Beneficios = beneficios,
            DataPublicacao = DateTime.Now,
            EmpresaId = int.Parse(empresaId)
        };

        _context.Vagas.Add(vaga);
        _context.SaveChanges();

        // Retorna para a view (o JS já faz o redirecionamento)
        return View();
    }
    // Action para exibir a página de busca de vagas
    [HttpGet]
    public IActionResult FeedProcuraVaga()
    {
        return View();
    }

    

    [HttpGet]
    public IActionResult FeedVaga(string cargo, string localizacao)
    {
        var vagas = _context.Vagas
            .Where(v =>
                (string.IsNullOrEmpty(cargo) || v.Titulo.Contains(cargo) || v.Empresa.AreaAtuacao.Contains(cargo)) &&
                (string.IsNullOrEmpty(localizacao) || v.Localizacao.Contains(localizacao))
            )
            .Select(v => new VagaViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo,
                Descricao = v.Descricao,
                Localizacao = v.Localizacao,
                Salario = v.Salario,
                Beneficios = v.Beneficios,
                DataPublicacao = v.DataPublicacao,
                Setor = v.Empresa.AreaAtuacao,
                NomeFantasia = v.Empresa.NomeFantasia
            })
            .ToList();

        var model = new FeedVagaViewModel
        {
            Vagas = vagas ?? new List<VagaViewModel>()
        };

        return View(model);
    }

}
