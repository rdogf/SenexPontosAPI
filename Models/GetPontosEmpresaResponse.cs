namespace SenexPontosAPI.Models
{
    public class GetPontosEmpresaResponse
    {
        public int id_pessoa { get; set; }
        public string nome { get; set; }
        public long cpf { get; set; }
        public int saldo_de_pontos { get; set; }
        public DateTime? data_atualizacao { get; set; }
        public decimal TotalValor { get; set; }
        public int TotalPontos { get; set; }
        public DateTime? UltimoConsumo { get; set; }
        public string nome_empresa { get; set; }
    }

}
