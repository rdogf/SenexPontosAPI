namespace SenexPontosAPI.Models
{
    public class CreditarPontosRequest
    {
        public int id_consumo { get; set; }
        public int id_pessoa { get; set; }
        public decimal valor_total { get; set; }
    }
}
