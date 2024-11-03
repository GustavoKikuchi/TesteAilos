using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Domain.Interfaces
{
    public interface IIdempotenceRepository
    {
        Task<GetIdempotenceResponseStore> Get(GetIdempotenceByIdRequestStore request);
        Task<int> Insert(InsertIdempotenceRequestStore request);
    }
}
