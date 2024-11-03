using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Infrastructure.TableMap
{
    public class Movimento
    {
        [Column("idmovimento")]
        public Guid IdMovimento { get; set; }
        [Column("idcontacorrente")]
        public Guid IdContaCorrente { get; set; }
        [Column("datamovimento")]
        public string DataMovimento { get; set; }
        [Column("tipomovimento")]
        public char TipoMovimento { get; set; }
        [Column("valor")]
        public double Valor { get; set; }
    }
}
