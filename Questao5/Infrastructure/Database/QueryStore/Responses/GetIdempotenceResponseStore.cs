using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetIdempotenceResponseStore
    {
        [Column("chave_idempotencia")]
        public Guid IdIdempotencia { get; set; }

        [Column("requisicao")]
        public string Requisicao { get; set; }

        [Column("resultado")]
        public string Resultado { get; set; }
    }
}
