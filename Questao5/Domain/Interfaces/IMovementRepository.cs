using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Domain.Interfaces
{
    public interface IMovementRepository
    {
        Task<int> Insert(InsertMovementRequestStore request);
    }
}
