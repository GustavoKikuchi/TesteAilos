using System.Text.Json.Serialization;

namespace Questao5.Application
{
    public class ErrorResponse
    {        
        [JsonPropertyName("error")]
        public string ErrorId { get; set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }
    }
}
