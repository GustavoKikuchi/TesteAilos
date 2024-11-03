using FluentAssertions;
using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5Tests;

[TestFixture]
public class GetBalanceQueryHandlerTests
{
    private IAccountRepository _accountRepositoryMock;
    private GetBalanceQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _accountRepositoryMock = Substitute.For<IAccountRepository>();
        _handler = new GetBalanceQueryHandler(_accountRepositoryMock);
    }

    [Test]
    public async Task Handle_WhenAccountIsNull_ShouldReturnInvalidResponse()
    {
        var queryRequest = new GetBalanceQuery(Guid.NewGuid());
        _accountRepositoryMock.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
                              .Returns((GetAccountResponseStore)null);

        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INVALID_ACCOUNT");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Apenas contas correntes cadastradas podem consultar o saldo");
    }

    [Test]
    public async Task Handle_WhenAccountIsInactive_ShouldReturnInvalidResponse()
    {
        var queryRequest = new GetBalanceQuery(Guid.NewGuid());
        var inactiveAccountResponse = new GetAccountResponseStore { Ativo = false };
        _accountRepositoryMock.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
                              .Returns(inactiveAccountResponse);

        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.FirstOrDefault().ErrorId.Should().Be("INACTIVE_ACCOUNT");
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("Apenas contas correntes ativas podem consultar o saldo");
    }

    [Test]
    public async Task Handle_WhenAccountIsActive_ShouldReturnBalance()
    {
        var queryRequest = new GetBalanceQuery(Guid.NewGuid());
        var activeAccountResponse = new GetAccountResponseStore { Ativo = true };
        var balanceResponse = new GetBalanceResponseStore { Nome = "Test User", Numero = 12345, Saldo = 100.567 };

        _accountRepositoryMock.GetIsValidAccount(Arg.Any<GetAccountByIdRequestStore>())
                              .Returns(activeAccountResponse);
        _accountRepositoryMock.GetBalance(Arg.Any<GetAccountByIdRequestStore>())
                              .Returns(balanceResponse);

        var result = await _handler.Handle(queryRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.NomeTitular.Should().Be("Test User");
        result.Data.NumeroConta.Should().Be(12345);
        result.Data.Saldo.Should().Be(100.57);
    }
}