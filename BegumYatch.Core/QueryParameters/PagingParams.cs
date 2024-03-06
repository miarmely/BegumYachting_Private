using System.ComponentModel.DataAnnotations;

namespace BegumYatch.Core.QueryParameters
{
    public record PagingParams
    {
        [Required] public int PageSize{ get; init; }
        [Required] public int PageNumber { get; init; }
    }
}
