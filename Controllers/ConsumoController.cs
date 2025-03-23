using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SenexPontosAPI.Business;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

[ApiController]
[Route("api/consumo")]
public class ConsumoController : ControllerBase
{
    private readonly AppManagers _mgr;

    public ConsumoController(AppManagers mgr)
    {
        _mgr = mgr;
    }
    [HttpPost("registrar-consumo")]
    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarConsumo([FromBody] TempoRealRequest request)
    {
        try
        {
            var resultado = await _mgr.consumoManager.RegistrarConsumoCompletoAsync(request);
            return Ok(resultado);
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro no RegistrarConsumo: " + ex.Message);
            return StatusCode(500, new { erro = ex.Message });
        }
    }

    [HttpGet("saldo/{id_pessoa}")]
    public async Task<IActionResult> GetPontosPorPessoa(int id_pessoa)
    {
        try
        {
            var pontos = await _mgr.consumoManager.GetPontosPorPessoa(id_pessoa);
            return Ok(pontos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = "Erro ao buscar pontos da pessoa.", detalhe = ex.Message });
        }
    }


}
