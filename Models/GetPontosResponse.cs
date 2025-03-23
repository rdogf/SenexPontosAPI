namespace SenexPontosAPI.Models
{
    public class GetPontosResponse
    {
        public int id_pessoa { get; set; }
        public string nome { get; set; }
        public long cpf { get; set; }
        public int saldo_de_pontos { get; set; }
        public DateTime data_atualizacao { get; set; }
        public int id_consumo { get; set; }
        public DateTime data_consumo { get; set; }
        public decimal valor_total { get; set; }
        public int pontos_obtidos_na_transacao { get; set; }
        public DateTime? data_criacao_registro { get; set; }
        public string nome_empresa { get; set; }
    }

}
