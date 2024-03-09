using System.Reflection.Metadata.Ecma335;

namespace BegumYatch.Core.Models.AdminPanel
{
    public record ExcursionDemandModel : BaseDemandAndOrderModel
    {
        public DateTime? ExcursionDate { get; init; }
        public int? NumberOfPeople { get; init; }
        public string? From { get; init; }
        public string? To { get; init; }
        public string? VehicleType { get; init; }
        public string? FirstLanguage { get; init; }
        public string? SecondLanguage { get; init; }
        public string? AccountOps { get; init; }
        public string? Notes { get; init; }
    }
}
