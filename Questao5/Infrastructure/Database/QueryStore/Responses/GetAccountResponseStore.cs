using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetAccountResponseStore
    {
        [Column("ativo")]
        public bool Ativo { get; set; }
    }
}
