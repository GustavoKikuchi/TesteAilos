
using MediatR;
using Questao5.Application.Queries.Responses;
using System.Text.Json.Serialization;

namespace Questao5.Application.Queries.Requests
{
    public class GetBalanceQuery : IRequest<BaseResponse<GetBalanceResponse>>
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        public GetBalanceQuery(Guid id)
        {
            this.Id = id;
        }
    }
}
