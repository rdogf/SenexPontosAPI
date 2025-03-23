using Microsoft.AspNetCore.Mvc;
using SenexPontosAPI.Models;
using SenexPontosAPI.Services;
using System.Text;

namespace SenexPontosAPI.Business
{
    public class PontosManager
    {
        private readonly IDapperHelper _dapper;

        public PontosManager(IDapperHelper dapper)
        {
            _dapper = dapper;
        }



        public async Task<string> GetSaldoHtml(int id_pessoa = 0, long cpf = 0)
        {
            var result = await _dapper.QueryAsync<GetPontosResponse>(
                "GetPontosPorPessoa",
                new { id_pessoa, cpf }
            );

            if (!result.Any())
                return "<h3>Nenhum registro de pontos encontrado para esse filtro.</h3>";

            var html = new StringBuilder();

            html.Append(StyleTable());

            html.Append($"<h3>Histórico de Pontos</h3>");

            html.Append("<table>");
            html.Append("<thead><tr>" +
                "<th>ID Pessoa</th>" +
                "<th>Nome</th>" +
                "<th>CPF</th>" +
                "<th>Empresa</th>" +
                "<th>Saldo</th>" +
                "<th>Atualizado em</th>" +
                "<th>ID Consumo</th>" +
                "<th>Data Consumo</th>" +
                "<th>Valor</th>" +
                "<th>Pontos Obtidos</th>" +
                "<th>Registrado em</th>" +
                "</tr></thead><tbody>");

            decimal totalValor = 0;
            int totalPontos = 0;

            foreach (var ponto in result)
            {
                totalValor += ponto.valor_total;
                totalPontos += ponto.pontos_obtidos_na_transacao;

                html.Append("<tr>" +
                    $"<td>{ponto.id_pessoa}</td>" +
                    $"<td>{ponto.nome}</td>" +
                    $"<td>{ponto.cpf}</td>" +
                    $"<td>{ponto.nome_empresa}</td>" +
                    $"<td>{ponto.saldo_de_pontos}</td>" +
                    $"<td>{(ponto.data_atualizacao > DateTime.MinValue ? ponto.data_atualizacao.ToString("dd/MM/yyyy HH:mm") : "-")}</td>" +
                    $"<td>{ponto.id_consumo}</td>" +
                    $"<td>{(ponto.data_consumo > DateTime.MinValue ? ponto.data_consumo.ToString("dd/MM/yyyy") : "-")}</td>" +
                    $"<td>{ponto.valor_total:C}</td>" +
                    $"<td>{ponto.pontos_obtidos_na_transacao}</td>" +
                    $"<td>{(ponto.data_criacao_registro.HasValue ? ponto.data_criacao_registro.Value.ToString("dd/MM/yyyy HH:mm") : "-")}</td>" +
                    "</tr>");
            }

            html.Append("</tbody><tfoot><tr>" +
                "<td colspan='8'>Totais</td>" +
                $"<td>{totalValor:C}</td>" +
                $"<td>{totalPontos}</td>" +
                "<td></td>" +
                "</tr></tfoot>");
            html.Append("</table>");

            return html.ToString();
        }


        public async Task<string> GetSaldoEmpresaHtml(int id_empresa = 0, long cnpj = 0)
        {
            var result = await _dapper.QueryAsync<GetPontosEmpresaResponse>(
                "GetPontosPorEmpresa",
                new { id_empresa, cnpj }
            );

            if (!result.Any())
                return "<h3>Nenhum consumidor encontrado para essa empresa.</h3>";

            var nomeEmpresa = result.First().nome_empresa;
            var totalValor = result.Sum(r => r.TotalValor);
            var totalPontos = result.Sum(r => r.TotalPontos);

            var html = new StringBuilder();
            html.Append(StyleTable());

            html.Append($"<h3>Resumo de Pontos - <span style='color:darkblue'>{nomeEmpresa}</span></h3>");

            html.Append("<table>");
            html.Append("<thead><tr>" +
                "<th>Empresa</th>" +
                "<th>Nome</th>" +
                "<th>CPF</th>" +
                "<th>Saldo Atual</th>" +
                "<th>Atualizado em</th>" +
                "<th>Último Consumo</th>" +
                "<th>Total Valor</th>" +
                "<th>Total Pontos</th>" +
                "</tr></thead><tbody>");

            foreach (var item in result)
            {
                html.Append("<tr>" +
                    $"<td>{item.nome_empresa}</td>" +
                    $"<td>{item.nome}</td>" +
                    $"<td>{item.cpf}</td>" +
                    $"<td>{item.saldo_de_pontos}</td>" +
                    $"<td>{item.data_atualizacao:dd/MM/yyyy HH:mm}</td>" +
                    $"<td>{item.UltimoConsumo:dd/MM/yyyy}</td>" +
                    $"<td>{item.TotalValor:C}</td>" +
                    $"<td>{item.TotalPontos}</td>" +
                    "</tr>");
            }

            html.Append("</tbody><tfoot><tr>" +
                "<td colspan='6'>Total Geral</td>" +
                $"<td>{totalValor:C}</td>" +
                $"<td>{totalPontos}</td>" +
                "</tr></tfoot>");
            html.Append("</table>");

            return html.ToString();
        }


        public async Task ProcessarPontosNoturnos()
        {
            await _dapper.ExecuteAsync("ProcessarPontosNoturnos", new { });
        }

        private string StyleTable()
        {
            var html = new StringBuilder();
            html.Append(@"<style>
                            table {
                                width: 100%;
                                border-collapse: collapse;
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                            }
                            th {
                                background-color: #f4f4f4;
                                text-align: left;
                                padding: 8px 12px;
                                border-bottom: 2px solid #ccc;
                            }
                            td {
                                padding: 8px 12px;
                                border-bottom: 1px solid #eee;
                            }
                            tr:hover {
                                background-color: #f9f9f9;
                            }
                            h3 {
                                font-family: Arial, sans-serif;
                                color: #333;
                            }
                            tfoot td {
                                font-weight: bold;
                                background-color: #fafafa;
                                border-top: 2px solid #ccc;
                            }
                        </style>");

        return html.ToString();

        }
    }

}
