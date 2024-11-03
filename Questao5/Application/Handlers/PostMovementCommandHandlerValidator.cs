using FluentValidation;
using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Handlers
{
    public class PostMovementCommandHandlerValidator : AbstractValidator<PostMovementCommandRequest>
    {
        public PostMovementCommandHandlerValidator()
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("Apenas valores positivos podem ser recebidos.").WithErrorCode("INVALID_VALUE");
            RuleFor(x => x.TipoMovimento)
                .Must(tipo=>tipo=='C'|| tipo == 'D').WithMessage("Apenas os tipos “débito” (D) ou “crédito” (C) podem ser aceitos").WithErrorCode("INVALID_TYPE");
        }
    }
}
