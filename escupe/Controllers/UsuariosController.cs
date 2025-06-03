using escupe.Data;
using escupe.Models;
using escupe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using escupe.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICPFService _cpfService;
    private readonly ICNPJService _cnpjService;
    private readonly ICEPService _cepService;

    public UsuariosController(
        ApplicationDbContext context,
        ICPFService cpfService,
        ICNPJService cnpjService,
        ICEPService cepService)
    {
        _context = context;
        _cpfService = cpfService;
        _cnpjService = cnpjService;
        _cepService = cepService;
    }

    [HttpGet]
    public IActionResult CadastrarCandidato(string tipoUsuario)
    {
        var model = new CadastroCandidatoViewModel
        {
            TipoUsuario = tipoUsuario
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarCandidato(CadastroCandidatoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.CEP = new string(model.CEP.Where(char.IsDigit).ToArray());
        var (isCepValido, enderecoViaCepResult) = await _cepService.ValidarCEP(model.CEP);
        if (!isCepValido)
        {
            ModelState.AddModelError("CEP", "O CEP informado é inválido.");
            return View(model);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var endereco = new Endereco
            {
                Logradouro = model.Logradouro,
                Numero = model.Numero,
                Complemento = model.Complemento,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                UF = model.UF,
                CEP = model.CEP
            };
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var candidato = new Candidato
            {
                NomeCompleto = model.NomeCompleto,
                CPF = model.CPF,
                Email = model.Email,
                Senha = model.Senha,
                Telefone = model.Telefone,
                EnderecoId = endereco.Id,
                TipoUsuario = model.TipoUsuario // <-- Adicione esta linha
            };
            _context.Candidato.Add(candidato);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            TempData["MensagemSucesso"] = "Cadastro de candidato realizado com sucesso!";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine("Erro ao salvar candidato: " + ex.Message);
            ModelState.AddModelError("", "Ocorreu um erro ao salvar os dados. Tente novamente.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult FormularioEmpresaEtapa1()
    {
        return PartialView("_FormularioEmpresaPartial1", new CadastroEmpresaViewModel());
    }

    [HttpPost]
    public IActionResult FormularioEmpresaEtapa1(CadastroEmpresaLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return PartialView("_FormularioEmpresaPartial1", model);

        TempData["Etapa1"] = JsonConvert.SerializeObject(model);
        return RedirectToAction("CadastrarEmpresa");
    }






    [HttpGet]
    public IActionResult CadastrarEmpresa(int etapa = 1, string tipoUsuario = "E")
    {
        var model = new CadastroEmpresaViewModel();
        model.TipoUsuario = tipoUsuario;
        ViewBag.Etapa = etapa;

        if (etapa == 2)
        {
            model.Email = TempData["Email"] as string;
            model.Telefone = TempData["Telefone"] as string;
            model.Senha = TempData["Senha"] as string;
            model.TipoUsuario = TempData["TipoUsuario"] as string;
            // Mantém os dados no TempData para o próximo request
            TempData.Keep();
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarEmpresa(CadastroEmpresaViewModel model, int etapa, string tipoUsuario)
    {
        ViewBag.Etapa = etapa;

        if (etapa == 1)
        {
            // Valida apenas os campos da etapa 1
            ModelState.Remove(nameof(model.NomeFantasia));
            ModelState.Remove(nameof(model.RazaoSocial));
            ModelState.Remove(nameof(model.CNPJ));
            ModelState.Remove(nameof(model.Logradouro));
            ModelState.Remove(nameof(model.Numero));
            ModelState.Remove(nameof(model.Bairro));
            ModelState.Remove(nameof(model.Cidade));
            ModelState.Remove(nameof(model.UF));
            ModelState.Remove(nameof(model.CEP));
            ModelState.Remove(nameof(model.AreaAtuacao));
            ModelState.Remove(nameof(model.Complemento));
            ModelState.Remove(nameof(model.TipoUsuario));



            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Salva os dados da etapa 1 temporariamente (TempData)
            TempData["Email"] = model.Email;
            TempData["Telefone"] = model.Telefone;
            TempData["Senha"] = model.Senha;
            TempData["TipoUsuario"] = model.TipoUsuario;

            // Redireciona para GET da etapa 2
            return RedirectToAction("CadastrarEmpresa", new { etapa = 2 });
        }
        // Valida todos os campos
        if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Aqui segue o fluxo de salvar no banco, igual ao seu código atual
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var (cepValido, enderecoViaCep) = await _cepService.ValidarCEP(model.CEP);
                if (!cepValido)
                {
                    ModelState.AddModelError("CEP", "O CEP informado é inválido.");
                    return View(model);
                }

                var endereco = new Endereco
                {
                    Logradouro = enderecoViaCep.Logradouro,
                    Numero = model.Numero,
                    Complemento = model.Complemento,
                    Bairro = enderecoViaCep.Bairro,
                    Cidade = enderecoViaCep.Localidade,
                    UF = enderecoViaCep.UF,
                    CEP = model.CEP
                };
                _context.Enderecos.Add(endereco);
                await _context.SaveChangesAsync();

                var empresa = new Empresa
                {
                    NomeFantasia = model.NomeFantasia,
                    CNPJ = model.CNPJ,
                    Email = model.Email,
                    Senha = model.Senha,
                    Telefone = model.Telefone,
                    EnderecoId = endereco.Id,
                    TipoUsuario = model.TipoUsuario
                };
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["MensagemSucesso"] = "Cadastro de empresa realizado com sucesso!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Erro ao salvar empresa: " + ex.Message);
                ModelState.AddModelError("", "Ocorreu um erro ao salvar os dados. Tente novamente.");
                return View(model);
            }
        

        // Caso etapa inválida
        return View(model);
    }
}
