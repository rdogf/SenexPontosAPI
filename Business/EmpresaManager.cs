using Azure.Core;
using Microsoft.Data.SqlClient;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

namespace SenexPontosAPI.Business
{
    public class EmpresaManager
    {
        private readonly IDapperHelper _dapper;

        public EmpresaManager(IDapperHelper dapper)
        {
            _dapper = dapper;
        }
        public async Task<IEnumerable<EmpresaModel>> GetEmpresa(long cnpj = 0, int id_empresa = 0)
        {
            var data = await _dapper.QueryAsync<EmpresaModel>("GetEmpresa", new { cnpj, id_empresa });

            if (!data.Any())
                throw new Exception("Não foi encontrado nenhuma Empresa!");

            return  data;
        }

        public async Task<EmpresaModel> GetEmpresaPorCnpj(long cnpj) 
        { 

            var data = await GetEmpresa(cnpj);

            if (!data.Any())
                throw new Exception("Empresa não encontrada com o CNPJ informado.");

            return data.First();
        }

        public async Task<EmpresaModel> GetEmpresaPorId(int id_empresa)
        {

            var data = await GetEmpresa(id_empresa);

            if (!data.Any())
                throw new Exception("Empresa não encontrada com o ID informado.");

            return data.First();
        }

        public async Task SetRegistrarEmpresaPorCnpj(EmpresaModel model)
        {
            try
            {
                await _dapper.ExecuteAsync("SetRegistrarEmpresaPorCnpj", model);
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                // Erro customizado lançado pela procedure
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar empresa: " + ex.Message);
            }
        }

    }
}
