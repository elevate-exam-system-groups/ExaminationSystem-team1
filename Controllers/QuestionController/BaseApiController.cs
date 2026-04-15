using ExaminationSystem.Controllers.Shared.Enums;
using ExaminationSystem.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.QuestionController
{
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult<T>(RequestResult<T> result, int successStatusCode = 200)
            where T : class
        {
            if (result.IsSuccess)
            {
                var response = ResponseViewModel<T>.Success(result.Data, result.Message);
                return successStatusCode == 201
                    ? Created(string.Empty, response)
                    : Ok(response);
            }

            return result.requestErrorCode switch
            {
                RequestErrorCode.ValidationError =>
                UnprocessableEntity(ResponseViewModel<T>.Failure(result.Message, ResponseVmErrorCode.ValidationError)),

                RequestErrorCode.Conflict =>
                Conflict(ResponseViewModel<T>.Failure(result.Message, ResponseVmErrorCode.Conflict)),

                RequestErrorCode.UserNotFound =>
                NotFound(ResponseViewModel<T>.Failure(result.Message, ResponseVmErrorCode.UserNotFound)),

                _ => BadRequest(ResponseViewModel<T>.Failure(result.Message, ResponseVmErrorCode.InternalServerError))
            };
        }
    }

}