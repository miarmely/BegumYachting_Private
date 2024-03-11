using Entities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BegumYatch.Core.QueryParameters
{
    public record LoginParamsForVerifyCode
    {
        [Required]
        [MiarEmail]
        public string Email { get; init; }

        [Required]
        [MiarLength(6, 6, "Doğrulama Codu", "Verify Code")]
        public string Code { get; init; }
    }
}
