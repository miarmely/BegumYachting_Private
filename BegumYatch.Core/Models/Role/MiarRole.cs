using System.Data;

namespace BegumYatch.Core.Models.Role
{
    public record MiarRole
    {
        public byte? Id { get; init; }
        public string? RoleName { get; init; }
    }
}
