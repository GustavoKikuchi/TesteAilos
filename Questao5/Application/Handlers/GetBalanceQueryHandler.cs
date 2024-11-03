using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public class GetBalanceQueryHandler : ApplicationBase, IRequestHandler<GetBalanceQuery, BaseResponse<GetBalanceResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        public GetBalanceQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<BaseResponse<GetBalanceResponse>> Handle(GetBalanceQuery queryRequest, CancellationToken cancellationToken)
        {
            var request = new GetAccountByIdRequestStore(queryRequest.Id);
            
            var accountResponse = await _accountRepository.GetIsValidAccount(request);

            if (accountResponse == null)
                return GetInvalidResponse<GetBalanceResponse>("INVALID_ACCOUNT", "Apenas contas correntes cadastradas podem consultar o saldo");

            if (!accountResponse.Ativo)
                return GetInvalidResponse<GetBalanceResponse>("INACTIVE_ACCOUNT", "Apenas contas correntes ativas podem consultar o saldo");

            var balance = await _accountRepository.GetBalance(request);

            return GetResponse(new GetBalanceResponse(balance.Nome, balance.Numero, Math.Round(balance.Saldo, 2)));
        }
    }
}
