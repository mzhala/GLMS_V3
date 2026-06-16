using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GLMS.Models.Enums;

namespace GLMS.Models
{
    public class Contract
    {
        [Display(Name = "Contract Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus? Status { get; set; }

        [Required]
        [Display(Name = "Service Level")]
        public ServiceLevel? ServiceLevel { get; set; }

       
        [Display(Name = "Agreement File")]
        public string? AgreementFilePath { get; set; }

        // Navigation Properties
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();
    }
}