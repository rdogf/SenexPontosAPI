using Microsoft.AspNetCore.Mvc;
using SenexPontosAPI.Business;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

namespace SenexPontosAPI.Controllers
{
    [ApiController]
    [Route("api/pessoa")]
    public class PessoaController : ControllerBase
    {
        private readonly AppManagers _mgr;

        public PessoaController(AppManagers mgr)
        {
            _mgr = mgr;
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetPessoa(long cpf)
        {
            try
            {
                var pessoa = await _mgr.pessoaManager.GetPessoaPorCpf(cpf);
                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                return NotFound(new { erro = ex.Message });
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPessoa([FromBody] PessoaModel model)
        {
            try
            {
                await _mgr.pessoaManager.SetRegistrarPessoaPorCpf(model);
                return Ok(new { mensagem = "Pessoa registrada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }


}
