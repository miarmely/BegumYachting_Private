using System.Text.Json;


namespace BegumYatch.Core.DTOs.Error
{
    public class MiarException : Exception
    {
        public MiarException(ErrorDto errorDto)
            : base(JsonSerializer.Serialize(errorDto))
        { }

        public MiarException(
            int statusCode,
            string errorCode,
            string errorDescription,
            string errorMessage)
            : base(JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorCode = errorCode,
                ErrorDescription = errorDescription,
                ErrorMessage = errorMessage
            }))
        {}
    }
}
