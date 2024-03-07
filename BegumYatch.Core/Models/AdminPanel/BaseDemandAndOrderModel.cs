using BegumYatch.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BegumYatch.Core.Models.AdminPanel
{
    public record BaseDemandAndOrderModel
    {
        [Key]
        public int? FormId { get; init; }
        public int? AnswererId { get; init; }
        public int? UserId { get; init; }  // id of form owner
        public string? NameSurname { get; init; }
        public string? YachtName { get; init; }
        public YacthType? YachtType { get; init; }
        public string? Flag { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? AnsweredDate { get; init; }
    }
}