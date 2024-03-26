using System.ComponentModel.DataAnnotations;

namespace TopUp_Beneficiary.Models
{
    public class TopUpBeneficiary
    {
        public int TopUpBeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }

        [MaxLength(20)]
        public string Nickname { get; set; }
        public int UserId { get; set; }

        public string BeneficiaryPhoneNumber { get; set; }
    
    }
}
