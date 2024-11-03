using MediatR;
using Questao5.Application.Commands.Response;
using System.Text.Json.Serialization;

namespace Questao5.Application.Commands.Requests
{
    public class PostMovementCommandRequest : IRequest<BaseResponse<PostMovementCommandResponse>>
    {
        [JsonPropertyName("request_id")]
        public Guid IdIdempotencia { get; set; }

        [JsonPropertyName("account_id")]
        public Guid IdConta { get; set; }

        [JsonPropertyName("transaction_type")]
        public char TipoMovimento { get; set; }

        [JsonPropertyName("value")]
        public double Valor { get; set; }
    }
}
