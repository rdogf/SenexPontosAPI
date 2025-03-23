using Microsoft.AspNetCore.Mvc;
using SenexPontosAPI.Business;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

namespace SenexPontosAPI.Controllers
{
    [ApiController]
    [Route("api/empresa")]
    public class EmpresaController : ControllerBase
    {
        private readonly AppManagers _mgr;

        public EmpresaController(AppManagers mgr)
        {
            _mgr = mgr;
        }

        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetEmpresa(long cnpj)
        {
            try
            {
                var empresa = await _mgr.empresaManager.GetEmpresaPorCnpj(cnpj);
                return Ok(empresa);
            }
            catch (Exception ex)
            {
                return NotFound(new { erro = ex.Message });
            }
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEmpresa([FromBody] EmpresaModel model)
        {
            try
            {
                await _mgr.empresaManager.SetRegistrarEmpresaPorCnpj(model);
                return Ok(new { mensagem = "Empresa registrada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }

}
