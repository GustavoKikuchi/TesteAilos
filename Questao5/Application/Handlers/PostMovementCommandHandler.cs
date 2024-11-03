using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public class PostMovementCommandHandler : ApplicationBase, IRequestHandler<PostMovementCommandRequest, BaseResponse<PostMovementCommandResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IIdempotenceRepository _idempotenceRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostMovementCommandHandler(IAccountRepository accountRepository,
            IIdempotenceRepository idempotenceRepository,
            IMovementRepository movementRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _idempotenceRepository = idempotenceRepository;
            _movementRepository = movementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<PostMovementCommandResponse>> Handle(PostMovementCommandRequest postRequest, CancellationToken cancellationToken)
        {
            var idempotence = await _idempotenceRepository.Get(new GetIdempotenceByIdRequestStore(postRequest.IdIdempotencia));
            var requisicaoString = JsonConvert.SerializeObject(postRequest);
            if (idempotence != null)
            {
                if (requisicaoString.Equals(idempotence.Requisicao))
                    return GetResponse(JsonConvert.DeserializeObject<PostMovementCommandResponse>(idempotence.Resultado));
                else
                    return GetInvalidResponse<PostMovementCommandResponse>("INVALID_TRANSACTION_ID", "Id da transação já utilizado");
            }

            var request = new GetAccountByIdRequestStore(postRequest.IdConta);

            var accountResponse = await _accountRepository.GetIsValidAccount(request);

            if (accountResponse == null)
                return GetInvalidResponse<PostMovementCommandResponse>("INVALID_ACCOUNT", "Apenas contas correntes cadastradas podem consultar o saldo");

            if (!accountResponse.Ativo)
                return GetInvalidResponse<PostMovementCommandResponse>("INACTIVE_ACCOUNT", "Apenas contas correntes ativas podem consultar o saldo");

            var movement = new InsertMovementRequestStore(Guid.NewGuid(), postRequest.IdConta, postRequest.TipoMovimento, postRequest.Valor);

            _unitOfWork.BeginTransaction();

            try
            {
                await _movementRepository.Insert(movement);

                await _idempotenceRepository.Insert(
                    new InsertIdempotenceRequestStore(postRequest.IdIdempotencia, requisicaoString, JsonConvert.SerializeObject(movement)));

                _unitOfWork.Commit();

                return GetResponse(new PostMovementCommandResponse(movement.Id));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return GetInvalidResponse<PostMovementCommandResponse>("INVALID_TRANSACTION", "Não foi possível realizar a transação. Tente novamente");
            }
        }
    }
}
