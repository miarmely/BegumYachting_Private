using System.Text.Json;

namespace BegumYatch.Core.DTOs.Error
{
    public class MiarException : Exception
    {
        public MiarException(object errorDetails) 
            : base(JsonSerializer.Serialize(errorDetails))
        {}
    }
}
