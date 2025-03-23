using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SenexPontosAPI.Business;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;
using System.Text;

[ApiController]
[Route("api/pontos")]
public class PontosController : ControllerBase
{
    private readonly AppManagers _mgr;

    public PontosController(AppManagers mgr)
    {
        _mgr = mgr;
    }

    [HttpGet("saldo")]
    public async Task<IActionResult> GetSaldoHtml([FromQuery] int id_pessoa = 0, [FromQuery] long cpf = 0)
    {
        var html = await _mgr.pontosManager.GetSaldoHtml(id_pessoa, cpf);
        return Content(html, "text/html; charset=utf-8");
    }

    [HttpGet("saldo-empresa")]
    public async Task<IActionResult> GetSaldoEmpresaHtml([FromQuery] int id_empresa = 0, [FromQuery] long cnpj = 0)
    {
        var html = await _mgr.pontosManager.GetSaldoEmpresaHtml(id_empresa, cnpj);
        return Content(html, "text/html; charset=utf-8");
    }

    [HttpPost("processar-noturno")]
    public async Task<IActionResult> ProcessarNoturno()
    {
        try
        {
            await _mgr.pontosManager.ProcessarPontosNoturnos();
            return Ok(new { mensagem = "Processamento noturno executado com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro ao processar pontos noturnos: {ex.Message}" });
        }
    }

    [HttpPost("tempo-real")]
    public async Task<IActionResult> CreditarTempoReal([FromBody] TempoRealRequest request)
    {
        try
        {
            var resultado = await _mgr.consumoManager.RegistrarConsumoTempoReal(request);
            return Ok(resultado);
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message });
        }
    }

}






