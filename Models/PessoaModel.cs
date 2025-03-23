namespace SenexPontosAPI.Models
{
    public class PessoaModel
    {
        public int id_pessoa { get; set; }
        public int id_empresa { get; set; }
        public long cpf { get; set; }
        public string nome { get; set; }
        public DateTime data_cadastro { get; set; }
    }
    public class IdPessoaResponse
    {
        public int id_pessoa { get; set; }
    }
}
