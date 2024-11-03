using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<GetBalanceResponseStore> GetBalance(GetAccountByIdRequestStore request);
        Task<GetAccountResponseStore> GetIsValidAccount(GetAccountByIdRequestStore request);        
    }
}
