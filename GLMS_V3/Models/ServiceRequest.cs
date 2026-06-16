using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GLMS.Models.Enums;

namespace GLMS.Models
{
    public class ServiceRequest
    {
        [Display(Name = "Service Request Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Contract Id")]
        public int ContractId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostUSD { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostZAR { get; set; }

        [Required]
        public ServiceRequestStatus Status { get; set; }

        // Navigation Property
        [ForeignKey("ContractId")]
        public Contract? Contract { get; set; }
    }
}