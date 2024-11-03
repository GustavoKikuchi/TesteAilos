namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class GetAccountByIdRequestStore
    {
        public Guid Id { get; set; }
        
        public GetAccountByIdRequestStore(Guid id)
        {
            Id = id;
        }
    }
}
