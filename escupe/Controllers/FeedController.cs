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
    // GET: Feed/EditarVaga/5
    [HttpGet]
    public IActionResult EditarVaga(int id)
    {
        var vaga = _context.Vagas.FirstOrDefault(v => v.Id == id);
        if (vaga == null)
            return NotFound();

        var model = new CriarVagaViewModel
        {
            Id = vaga.Id,
            Titulo = vaga.Titulo,
            Localizacao = vaga.Localizacao,
            Salario = vaga.Salario.ToString("F2"),
            Descricao = vaga.Descricao,
            Beneficios = vaga.Beneficios
        };

        return View(model); // Retorna a view EditarVaga.cshtml com o model preenchido
    }

    // POST: Feed/EditarVaga
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditarVaga(CriarVagaViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var vaga = _context.Vagas.FirstOrDefault(v => v.Id == model.Id);
        if (vaga == null)
            return NotFound();

        vaga.Titulo = model.Titulo;
        vaga.Localizacao = model.Localizacao;
        decimal salarioDecimal = 0;
        decimal.TryParse(model.Salario, out salarioDecimal);
        vaga.Salario = salarioDecimal;
        vaga.Descricao = model.Descricao;
        vaga.Beneficios = model.Beneficios;

        _context.SaveChanges();

        ViewBag.MensagemSucesso = "Vaga editada com sucesso!";
        return View(model); // Retorna para a mesma view
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Candidatar(
    int vagaId,
    string email,
    string telefone,
    IFormFile curriculo,
    string descricao)
    {
        // Recupera o id do candidato logado
        var candidatoIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (candidatoIdStr == null)
            return RedirectToAction("Login", "Home");

        int candidatoId = int.Parse(candidatoIdStr);

        // Salva o arquivo do currículo
        string curriculoPath = null;
        if (curriculo != null && curriculo.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(curriculo.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "curriculos");
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var filePath = Path.Combine(savePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await curriculo.CopyToAsync(stream);
            }
            curriculoPath = "/curriculos/" + fileName;
        }

        var candidatura = new Candidatura
        {
            VagaId = vagaId,
            CandidatoId = candidatoId,
            Email = email,
            Telefone = telefone,
            CurriculoPath = curriculoPath,
            Descricao = descricao,
            DataCandidatura = DateTime.Now,
            Status = "Pendente"
        };

        _context.Candidatura.Add(candidatura);
        await _context.SaveChangesAsync();

        return RedirectToAction("FeedVaga");
    }







    [HttpGet]
    public IActionResult MinhasVagas()
    {
        var empresaIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (empresaIdStr == null)
            return RedirectToAction("Login", "Home");

        int empresaId = int.Parse(empresaIdStr);

        var vagas = _context.Vagas
            .Where(v => v.EmpresaId == empresaId)
            .Select(v => new VagaComCandidatosViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo,
                Descricao = v.Descricao,
                Localizacao = v.Localizacao,
                Salario = v.Salario,
                Beneficios = v.Beneficios,
                DataPublicacao = v.DataPublicacao,
                Candidatos = _context.Candidatura
                    .Where(c => c.VagaId == v.Id)
                    .Select(c => new CandidatoViewModel
                    {
                        Id = c.Candidato.Id,
                        Nome = c.Candidato.NomeCompleto,
                        
                        DataCandidatura = c.DataCandidatura,
                        Status = c.Status,
                        CurriculoPath = c.CurriculoPath // Adicione este campo no ViewModel
                    }).ToList()
            }).ToList();

        return View(vagas);
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
        ModelState.Remove(nameof(model.RazaoSocial));
        ModelState.Remove(nameof(model.Site));
        ModelState.Remove(nameof(model.TipoUsuario));
        ModelState.Remove(nameof(model.Vagas));
        ModelState.Remove(nameof(model.Endereco));
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
