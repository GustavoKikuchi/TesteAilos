namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class InsertIdempotenceRequestStore
    {
        public InsertIdempotenceRequestStore(Guid id, string requisicao, string resultado)
        {
            Id = id;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public Guid Id { get; }
        public string Requisicao { get; }
        public string Resultado { get; }
    }
}
