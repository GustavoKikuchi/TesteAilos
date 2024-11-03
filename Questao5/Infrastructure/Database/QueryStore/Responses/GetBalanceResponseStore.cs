using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetBalanceResponseStore
    {
        [Column("numero")]
        public int Numero { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("saldo")]
        public double Saldo { get; set; }
    }
}
