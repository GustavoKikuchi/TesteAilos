using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Questao5.Application;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Application.Handlers;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit.Sdk;

namespace Questao5Tests;

[TestFixture]
public class PostMovementCommandHandlerTests
{
    private PostMovementCommandHandler _handler;
    private IIdempotenceRepository _idempotenceRepository;
    private IAccountRepository _accountRepository;
    private IMovementRepository _movementRepository;
    private IUnitOfWork _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        _idempotenceRepository = Substitute.For<IIdempotenceRepository>();
        _accountRepository = Substitute.For<IAccountRepository>();
        _movementRepository = Substitute.For<IMovementRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new PostMovementCommandHandler(
            _accountRepository,
            _idempotenceRepository,
            _movementRepository,
            _unitOfWork);
    }

    [TearDown]
    public void Dispose()
    {
        _unitOfWork.Dispose();
    }

    [Test]
    public async Task Handle_IdempotentRequestWithSameRequest_ReturnsCachedResponse()
    {
        var idIdempotencia = Guid.NewGuid();

        var postRequest = new PostMovementCommandRequest
        {
            IdIdempotencia = idIdempotencia,
            IdConta = Guid.NewGuid(),
            TipoMovimento = 'C',
            Valor = 100.0
        };

        var requisicaoString = JsonConvert.SerializeObject(postRequest);

        var response = new GetIdempotenceResponseStore
        {
            IdIdempotencia = idIdempotencia,
            Requisicao = requisicaoString,
            Resultado = JsonConvert.SerializeObject(new PostMovementCommandResponse(idIdempotencia))
        };

        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>()).Returns(response);

        var result = await _handler.Handle(postRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Test]
    public async Task Handle_IdempotentRequestWithDifferentRequest_ReturnsInvalidTransactionResponse()
    {
        var idIdempotencia = Guid.NewGuid();

        var postRequest = new PostMovementCommandRequest
        {
            IdIdempotencia = idIdempotencia,
            IdConta = Guid.NewGuid(),
            TipoMovimento = 'C',
            Valor = 100.0
        };

        var response = new GetIdempotenceResponseStore
        {
            IdIdempotencia = idIdempotencia,
            Requisicao = "DiferenteRequisicao",
            Resultado = JsonConvert.SerializeObject(new PostMovementCommandResponse(idIdempotencia))
        };

        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>()).Returns(response);

        var result = await _handler.Handle(postRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INVALID_TRANSACTION_ID");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Id da transação já utilizado");

    }

    [Test]
    public async Task Handle_InvalidAccount_ReturnsInvalidAccountResponse()
    {
        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>())
            .ReturnsNull();

        _accountRepository.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
            .ReturnsNull();

        var result = await _handler.Handle(new PostMovementCommandRequest(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INVALID_ACCOUNT");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Apenas contas correntes cadastradas podem consultar o saldo");
    }

    [Test]
    public async Task Handle_InactiveAccount_ReturnsInactiveAccountResponse()
    {
        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>())
           .ReturnsNull();

        _accountRepository.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
            .Returns(new GetAccountResponseStore { Ativo = false });

        var result = await _handler.Handle(new PostMovementCommandRequest(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INACTIVE_ACCOUNT");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Apenas contas correntes ativas podem consultar o saldo");
    }

    [Test]
    public async Task Handle_SuccessfulTransaction_CommitsTransactionAndReturnsResponse()
    {
        var postRequest = new PostMovementCommandRequest
        {
            IdIdempotencia = Guid.NewGuid(),
            IdConta = Guid.NewGuid(),
            TipoMovimento = 'C',
            Valor = 100.0
        };

        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>())
            .ReturnsNull();

        _accountRepository.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
            .Returns(new GetAccountResponseStore { Ativo = true });

        _movementRepository.Insert(Arg.Any<InsertMovementRequestStore>()).Returns(Task.FromResult(1));
        _idempotenceRepository.Insert(Arg.Any<InsertIdempotenceRequestStore>()).Returns(Task.FromResult(1));

        var result = await _handler.Handle(postRequest, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Errors.Should().BeNull();
        _unitOfWork.Received(1).BeginTransaction();
        _unitOfWork.Received(1).Commit();
    }

    [Test]
    public async Task Handle_FailedTransaction_RollsBackTransactionAndReturnsErrorResponse()
    {
        var postRequest = new PostMovementCommandRequest
        {
            IdIdempotencia = Guid.NewGuid(),
            IdConta = Guid.NewGuid(),
            TipoMovimento = 'C',
            Valor = 100.0
        };

        _idempotenceRepository.Get(Arg.Any<GetIdempotenceByIdRequestStore>())
            .ReturnsNull();

        _accountRepository.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
            .Returns(new GetAccountResponseStore { Ativo = true });

        _movementRepository.When(x => x.Insert(Arg.Any<InsertMovementRequestStore>()))
            .Do(x => throw new Exception("Database error"));

        var result = await _handler.Handle(postRequest, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INVALID_TRANSACTION");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Não foi possível realizar a transação. Tente novamente");
        _unitOfWork.Received(1).BeginTransaction();
        _unitOfWork.Received(1).Rollback();
    }
}
