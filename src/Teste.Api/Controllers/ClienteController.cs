using Microsoft.AspNetCore.Mvc;
using Teste.Api.Data;
using Teste.Api.Models;
using Teste.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Teste.Api.ViewModels.ResultsViewModels;
using Teste.Api.ViewModels.PaginacaoViewModels;
using Teste.Api.Services;

namespace Teste.Api.Controllers
{
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost("/clientes")]
        public async Task<IActionResult> CriarClienteAsync(
            [FromBody] ClienteViewModel model,
            [FromServices] TesteDbContext context,
            [FromServices] ViaCepService viaCepService)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var apiError = new ApiError("VALIDATION_ERROR", "dados inválidos", errors);

                return BadRequest(new ResultViewModel<Cliente>(apiError));
            }
            var nomeNormalizado = model.Nome.Trim();
            var emailNormalizado = model.Email.Trim().ToLower();
            var telefoneNormalizado = new string(model.Telefone.Where(char.IsDigit).ToArray());
            var cepNormalizado = new string(model.Cep.Where(char.IsDigit).ToArray());

            if (await context.Clientes.AnyAsync(c => c.Email == emailNormalizado))
            {
                var error = new ApiError("CONFLICT", "O email já está em uso");
                return Conflict(new ResultViewModel<Cliente>(error));
            }

            if (await context.Clientes.AnyAsync(c => c.Telefone == telefoneNormalizado))
            {
                var error = new ApiError("CONFLICT", "O telefone já está em uso");
                return Conflict(new ResultViewModel<Cliente>(error));
            }

            ViaCepResponse? enderecoViaCep;
            try
            {
                enderecoViaCep = await viaCepService.ObterEnderecoAsync(cepNormalizado);
                if (enderecoViaCep == null) {                     
                    var error = new ApiError("BAD_REQUEST", "Não foi possível validar o CEP informado");
                    return BadRequest(new ResultViewModel<Cliente>(error));
                }
            }
            catch (HttpRequestException)
            {
                var erroServico = new ApiError("SERVICE_UNAVAILABLE", "Erro ao consultar o serviço de CEP");
                return StatusCode(503, new ResultViewModel<Cliente>(erroServico));
            }

            try
            { 
                var cliente = new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = nomeNormalizado,
                    Email = emailNormalizado,
                    Telefone = telefoneNormalizado,
                    Cep = cepNormalizado,
                    Numero = model.Numero,
                    Complemento = model.Complemento,
                    Logradouro = enderecoViaCep.Logradouro,
                    Bairro = enderecoViaCep.Bairro,
                    Cidade = enderecoViaCep.Cidade,
                    Uf = enderecoViaCep.Uf,
                    CriadoEm = DateTime.UtcNow,
                    AtualizadoEm = DateTime.UtcNow
                };

                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();

                return Created($"/clientes/{cliente.Id}", new ResultViewModel<Cliente>(cliente));
            }
            catch (Exception)
            {
                var error = new ApiError("INTERNAL_ERROR", "Falha interna no servidor");
                return StatusCode(500, new ResultViewModel<Cliente>(error));
            }
        }

        [HttpGet("/clientes")]
        public async Task<IActionResult> ObterClientesAsync(
            [FromServices] TesteDbContext context,
            [FromQuery, Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior ou igual a 1")] int page = 1,
            [FromQuery, Range(1, 50, ErrorMessage = "O tamanho da página deve estar entre 1 e 50")] int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var apiError = new ApiError("VALIDATION_ERROR", "dados de consulta inválidos", errors);
                return BadRequest(new ResultViewModel<List<Cliente>>(apiError));
            }

            try
            {
                var totalItems = await context.Clientes.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var clientes = await context.Clientes
                    .AsNoTracking()
                    .OrderByDescending(c => c.CriadoEm)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var paginacao = new Paginacao
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                };
                return Ok(new PaginacaoResultViewModel<List<Cliente>>(clientes, paginacao));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Cliente>>(new ApiError("INTERNAL_ERROR", "Falha interna no servidor", new List<string> { ex.Message })));
            }
        }

        [HttpGet("/clientes/{id:guid}")]
        public async Task<IActionResult> ObterClientePorIdAsync(
            [FromRoute] Guid id,
            [FromServices] TesteDbContext context)
        {
            try
            {
                var cliente = await context.Clientes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    var error = new ApiError("NOT_FOUND", "Cliente não encontrado");
                    return NotFound(new ResultViewModel<Cliente>(error));
                }
                return Ok(new ResultViewModel<Cliente>(cliente));
            }
            catch (Exception ex)
            {
                var error = new ApiError("INTERNAL_ERROR", "Falha interna no servidor", new List<string> { ex.Message });
                return StatusCode(500, new ResultViewModel<Cliente>(error));
            }
        }

        [HttpPut("/clientes/{id:guid}")]
        public async Task<IActionResult> AtualizarClienteAsync(
            [FromRoute] Guid id,
            [FromBody] ClienteViewModel model,
            [FromServices] TesteDbContext context,
            [FromServices] ViaCepService viaCepService)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var apiError = new ApiError("VALIDATION_ERROR", "dados inválidos", errors);
                return BadRequest(new ResultViewModel<Cliente>(apiError));
            }
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
                if (cliente == null)
                {
                    var error = new ApiError("NOT_FOUND", "Cliente não encontrado");
                    return NotFound(new ResultViewModel<Cliente>(error));
                }
                var emailNormalizado = model.Email.Trim().ToLower();
                var telefoneNormalizado = new string(model.Telefone.Where(char.IsDigit).ToArray());
                var cepNormalizado = new string(model.Cep.Where(char.IsDigit).ToArray());
                if (await context.Clientes.AnyAsync(c => c.Email == emailNormalizado && c.Id != id))
                {
                    var error = new ApiError("CONFLICT", "O email já está em uso");
                    return Conflict(new ResultViewModel<Cliente>(error));
                }

                if (await context.Clientes.AnyAsync(c => c.Telefone == telefoneNormalizado && c.Id != id))
                {
                    var error = new ApiError("CONFLICT", "O telefone já está em uso");
                    return Conflict(new ResultViewModel<Cliente>(error));
                }
                if (cliente.Cep != cepNormalizado)
                {
                    ViaCepResponse? enderecoViaCep;
                    try
                    {
                        enderecoViaCep = await viaCepService.ObterEnderecoAsync(cepNormalizado);
                        if (enderecoViaCep == null)
                        {
                            var error = new ApiError("BAD_REQUEST", "Não foi possível validar o CEP informado");
                            return BadRequest(new ResultViewModel<Cliente>(error));
                        }
                    }
                    catch (HttpRequestException)
                    {
                        var erroCep = new ApiError("SERVICE_UNAVAILABLE", "Erro ao consultar o serviço de CEP");
                        return StatusCode(503, new ResultViewModel<Cliente>(erroCep));
                    }

                    cliente.Cep = cepNormalizado;
                    cliente.Logradouro = enderecoViaCep.Logradouro;
                    cliente.Bairro = enderecoViaCep.Bairro;
                    cliente.Cidade = enderecoViaCep.Cidade;
                    cliente.Uf = enderecoViaCep.Uf;
                }

                cliente.Nome = model.Nome.Trim();
                cliente.Email = emailNormalizado;
                cliente.Telefone = telefoneNormalizado;
                cliente.Numero = model.Numero;
                cliente.Complemento = model.Complemento;
                cliente.AtualizadoEm = DateTime.UtcNow;

                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<Cliente>(cliente));
            }
            catch (Exception ex)
            {
                var error = new ApiError("INTERNAL_ERROR", "Falha interna no servidor", new List<string> { ex.Message });
                return StatusCode(500, new ResultViewModel<Cliente>(error));
            }
        }
    }
}

