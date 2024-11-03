using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Questao5.Application
{
    public class BaseResponse<T>
    {
        public bool Success
        {
            get
            {
                return Errors == null ;
            }
        }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("errors")]
        public IEnumerable<ErrorResponse> Errors { get; set; }        
    }
}
