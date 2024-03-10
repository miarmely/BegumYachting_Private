namespace BegumYatch.Core.DTOs.ProvisionOrder
{
    public class AddProvisionOrderDto
    {
        public string? NameSurname { get; set; }  
        public string? SupplyPort { get; set; }
        public DateTime SupplyDate { get; set; }
        public string? AccountOps { get; set; }
        public string? Notes { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int CheckInId { get; set; }
    }
}
