using System.Text.Json;

namespace BegumYatch.Core.DTOs.Error
{
    public partial record ErrorDto
    {
        public int StatusCode { get; init; }
        public string ErrorCode { get; init; }
        public string ErrorDescription { get; init; }
        public string ErrorMessage { get; init; }
    }

    public partial record ErrorDto // functions
    {
        public object ToJson() => new
        {
            StatusCode,
            ErrorCode,
            ErrorDescription,
            ErrorMessage
        };
        public string toString() => 
            JsonSerializer.Serialize(this);
    }
}
