namespace SenexPontosAPI.Models
{
    public class TempoRealRequest
    {
        public long cnpj { get; set; }
        public long cpf { get; set; }
        public string nome { get; set; }
        public decimal valor_total { get; set; }
        public DateTime data_consumo { get; set; }
    }

}
