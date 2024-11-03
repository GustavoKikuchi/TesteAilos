namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class InsertMovementRequestStore
    {
        public InsertMovementRequestStore(Guid id, Guid idContaCorrente, char tipoMovimento, double valor)
        {
            Id = id;
            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string DataMovimento { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public char TipoMovimento { get; set; }
        public double Valor { get; set; }
    }
}
