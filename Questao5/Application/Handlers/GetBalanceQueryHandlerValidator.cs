using FluentValidation;
using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public class GetBalanceQueryHandlerValidator : AbstractValidator<GetBalanceQuery>
    {
        public GetBalanceQueryHandlerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id da conta é obrigatório.").WithErrorCode("INVALID_ACCOUNT");
        }
    }
}
