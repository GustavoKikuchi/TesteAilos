using FluentValidation.Results;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using Questao5.Application;

namespace Questao5.Domain.Exceptions
{
    public class BaseException<T> : Exception
    {
        public BaseResponse<T> Response { get; set; }

        public BaseException(IEnumerable<ValidationFailure> errors) : base()
        {
            var errorsResponse = new List<ErrorResponse>();

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    errorsResponse.Add(new ErrorResponse { ErrorId = error.ErrorCode, ErrorMessage = error.ErrorMessage });
                }
            }
            Response = new BaseResponse<T>();
            Response.Errors = errorsResponse;
        }
    }
}
