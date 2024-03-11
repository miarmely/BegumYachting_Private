using Entities.Attributes;


namespace BegumYatch.Core.QueryParameters
{
    public record LoginParamsForSendCodeToMail
    {
        [MiarEmail] public string Email { get; init; }
    }
}
