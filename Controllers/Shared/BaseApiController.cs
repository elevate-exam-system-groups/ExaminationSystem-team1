namespace ExaminationSystem.Controllers.Shared
{

    [ApiController]
    public class BaseApiController : ControllerBase
    {

        protected ActionResult<ResponseViewModel<T>> HandleResult<T>(
            RequestResult<T> result, int successStatusCode = 200)
        {

            if (result.IsSuccess)
            {
                return StatusCode(successStatusCode, ResponseViewModel<T>.Success(
                    result.Data!, result.Message));
            }

            return StatusCode(GetStatusCode(result.requestErrorCode), ResponseViewModel<T>.Failure(
                result.Message, 
                MapErrorCode(result.requestErrorCode)));
        }

        protected static ResponseVmErrorCode MapErrorCode(RequestErrorCode? code) => code switch
        {
            RequestErrorCode.NotFound => ResponseVmErrorCode.NotFound,
            RequestErrorCode.Conflict => ResponseVmErrorCode.Conflict,
            RequestErrorCode.ValidationError => ResponseVmErrorCode.ValidationError,
            _ => ResponseVmErrorCode.InternalServerError
        };

        protected static int GetStatusCode(RequestErrorCode? code) => code switch
        {
            RequestErrorCode.NotFound => 404,
            RequestErrorCode.Conflict => 409,
            RequestErrorCode.ValidationError => 422,
            _ => 500
        };

    }
}
