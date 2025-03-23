using Microsoft.Data.SqlClient;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

public class PessoaManager
{
    private readonly IDapperHelper _dapper;

    public PessoaManager(IDapperHelper dapper)
    {
        _dapper = dapper;
    }

    public async Task<PessoaModel> GetPessoaPorCpf(long cpf)
    {
        var data = await _dapper.QueryAsync<PessoaModel>("GetPessoa", new { cpf });

        if (!data.Any())
            throw new Exception("Pessoa não encontrada com o CPF informado.");

        return data.First();
    }

    public async Task SetRegistrarPessoaPorCpf(PessoaModel model)
    {
        try
        {
            await _dapper.ExecuteAsync("SetRegistrarPessoaPorCpf", model);
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao registrar pessoa: " + ex.Message);
        }
    }
}
