using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.TableMap
{
    public class ContaCorrente
    {
        [Column("idcontacorrente")]
        public Guid IdContaCorrente { get; set; }
        [Column("numero")]
        public int Numero { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("ativo")]
        public bool Ativo { get; set; }
    }
}
