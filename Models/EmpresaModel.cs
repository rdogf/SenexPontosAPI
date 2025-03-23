namespace SenexPontosAPI.Models
{
    public class EmpresaModel
    {
        public int id_empresa { get; set; }
        public long cnpj { get; set; }
        public string nome { get; set; }
        public DateTime? data_criacao { get; set; }
    }
}
