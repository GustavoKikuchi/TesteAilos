namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class GetIdempotenceByIdRequestStore
    {
        public Guid Id { get; set; }
        
        public GetIdempotenceByIdRequestStore(Guid id)
        {
            Id = id;
        }
    }
}
