using Re_ABP_Backend.Errors;

namespace API.Errors
{
    public class ApiErrorResponse : ApiResponse
    {
        public ApiErrorResponse(int statusCode, string? message = null, string details = null!) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }

}