using System.Text.Json.Serialization;

namespace Questao5.Application.Commands.Response
{
    public class PostMovementCommandResponse
    {
        public PostMovementCommandResponse(Guid id)
        {
            Id = id;
        }
        
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
