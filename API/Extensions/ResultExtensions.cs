using API.Errors;
using Application.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.Success)
                return new OkObjectResult(result.Value);

            return MapError(result.Error!);
        }

        public static ActionResult ToActionResult(this Result result)
        {
            if (result.Success)
                return new NoContentResult();

            return MapError(result.Error!);
        }

        private static ActionResult MapError(Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(new ApiResponse(404, error.Message)),
                ErrorType.Forbidden => new ObjectResult(new ApiResponse(403, error.Message)) { StatusCode = 403 },
                ErrorType.Unauthorized => new UnauthorizedObjectResult(new ApiResponse(401, error.Message)),
                ErrorType.BadRequest => new BadRequestObjectResult(new ApiResponse(400, error.Message)),
                ErrorType.Validation => new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new List<string> { error.Message }
                }),
                _ => new ObjectResult(new ApiResponse(500, error.Message)) { StatusCode = 500 }
            };
        }
    }
}
