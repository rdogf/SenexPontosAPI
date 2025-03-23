using SenexPontosAPI.Models;
using SenexPontosAPI.Services;

namespace SenexPontosAPI.Business
{
    public class ConsumoManager
    {
        private readonly IDapperHelper _dapper;
        private readonly EmpresaManager empresaManager;
        private readonly PessoaManager pessoaManager;

        public ConsumoManager(IDapperHelper dapper, EmpresaManager empresaManager, PessoaManager pessoaManager)
        {
            _dapper = dapper;
            this.empresaManager = empresaManager;
            this.pessoaManager = pessoaManager;
        }

        public async Task<object> RegistrarConsumoCompletoAsync(TempoRealRequest request)
        {
            // 1️⃣ Valida empresa
            var empresa = await empresaManager.GetEmpresaPorCnpj(request.cnpj);

            // 2️⃣ Registra (ou busca) pessoa
            var pessoas = await _dapper.QueryAsync<IdPessoaResponse>("SetRegistrarPessoaPorCpf", new
            {
                cpf = request.cpf,
                nome = request.nome,
                id_empresa = empresa.id_empresa
            });

            if (!pessoas.Any())
                throw new Exception("Erro ao registrar ou obter pessoa.");

            int id_pessoa = pessoas.First().id_pessoa;

            var pessoa = await pessoaManager.GetPessoaPorCpf(request.cpf);

            // 3️⃣ Registra consumo
            var consumos = await _dapper.QueryAsync<CreditarPontosRequest>("SetRegistrarConsumoDireto", new
            {
                id_pessoa_que_consumiu = id_pessoa,
                data_consumo = request.data_consumo,
                valor_total = request.valor_total
            });

            if (!consumos.Any())
                throw new Exception("Erro ao registrar consumo.");

            var consumo = consumos.First();

            // 4️⃣ Retorno formatado
            return new
            {
                mensagem = "Consumo registrado com sucesso.",
                id_consumo = consumo.id_consumo,
                pessoa = new
                {
                    pessoa.id_pessoa,
                    pessoa.nome,
                    pessoa.cpf,
                    empresa = new
                    {
                        empresa.id_empresa,
                        empresa.nome,
                        empresa.cnpj
                    }
                }
            };
        }

        public async Task<object> RegistrarConsumoTempoReal(TempoRealRequest request)
        {
            // 1️⃣ Valida empresa
            var empresa = await empresaManager.GetEmpresaPorCnpj(request.cnpj);

            // 2️⃣ Registra (ou busca) pessoa
            var pessoas = await _dapper.QueryAsync<IdPessoaResponse>("SetRegistrarPessoaPorCpf", new
            {
                cpf = request.cpf,
                nome = request.nome,
                id_empresa = empresa.id_empresa
            });

            if (!pessoas.Any())
                throw new Exception("Erro ao registrar ou obter pessoa.");

            int id_pessoa = pessoas.First().id_pessoa;

            // 3️⃣ Registra o consumo
            var consumos = await _dapper.QueryAsync<CreditarPontosRequest>("SetRegistrarConsumoDireto", new
            {
                id_pessoa_que_consumiu = id_pessoa,
                data_consumo = request.data_consumo,
                valor_total = request.valor_total
            });

            if (!consumos.Any())
                throw new Exception("Erro ao registrar consumo.");

            var consumo = consumos.First();

            // 4️⃣ Credita pontos
            await _dapper.ExecuteAsync("SetCreditarPontosTempoReal", new CreditarPontosRequest
            {
                id_consumo = consumo.id_consumo,
                id_pessoa = id_pessoa,
                valor_total = request.valor_total
            });

            // 5️⃣ Retorno estruturado
            var pessoa = await pessoaManager.GetPessoaPorCpf(request.cpf);

            return new
            {
                mensagem = "Consumo e pontos processados com sucesso.",
                id_consumo = consumo.id_consumo,
                pessoa = new
                {
                    pessoa.id_pessoa,
                    pessoa.nome,
                    pessoa.cpf,
                    empresa = new
                    {
                        empresa.id_empresa,
                        empresa.nome,
                        empresa.cnpj
                    }
                }
            };
        }


        public async Task<IEnumerable<GetPontosResponse>> GetPontosPorPessoa(int id_pessoa)
        {
            return await _dapper.QueryAsync<GetPontosResponse>("GetPontosPorPessoa", new { id_pessoa });
        }
    }

}
