using System.Text.Json.Serialization;

namespace Questao5.Application.Queries.Responses
{
    public class GetBalanceResponse
    {
        [JsonPropertyName("account_number")]
        public int NumeroConta { get; set; }
        
        [JsonPropertyName("account_holder_name")]
        public string NomeTitular { get; set; }

        [JsonPropertyName("query_date")]
        public string DataConsulta { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [JsonPropertyName("balance")]
        public double Saldo { get; set; }
        
        public GetBalanceResponse(string nome, int numero, double saldo)
        {
            NomeTitular = nome;
            NumeroConta = numero;
            Saldo = saldo;
        }
    }
}
