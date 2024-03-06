using BegumYatch.Core.Enums.AdminPanel;
using System.ComponentModel.DataAnnotations;


namespace BegumYatch.Core.QueryParameters
{
    public record FormParamsForDisplayFormByStatus : PagingParams
    {
        [Required] public FormStatus FormStatus { get; init; }
    }
}