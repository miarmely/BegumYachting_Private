using Entities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BegumYatch.Core.QueryParameters
{
    public record LoginParamsForVerifyCode
    {
        [Required] 
        public string Email { get; init; }

        [Required]
        [MiarLength(6, 6, "Doğrulama Codu", "Verification Code")]
        public string VerificationCode { get; init; }
    }
}
