using FluentValidation.Results;

namespace Questao5.Application
{
    public class ApplicationBase
    {                
        protected BaseResponse<T> GetResponse<T>(T obj)
        {
            return new BaseResponse<T>
            {
                Data = obj
            };
        }

        protected BaseResponse<T> GetInvalidResponse<T>(IList<ValidationFailure> errors)
        {
            var errorsResponse = new List<ErrorResponse>();

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    errorsResponse.Add(new ErrorResponse { ErrorId = error.ErrorCode, ErrorMessage = error.ErrorMessage });
                }
            }

            return new BaseResponse<T>
            {
                Errors = errorsResponse
            };
        }

        protected BaseResponse<T> GetInvalidResponse<T>(string error, string errorMessage)
        {
            var errorsResponse = new List<ErrorResponse>
            {
                new ErrorResponse { ErrorId = error, ErrorMessage = errorMessage }
            };
            
            return new BaseResponse<T>
            {
                Errors = errorsResponse
            };
        }
    }
}
