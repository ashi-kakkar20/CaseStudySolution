using System.ComponentModel.DataAnnotations.Schema;

namespace ExternalServiceBalanceTransaction.Models
{
    public class BalanceInformation
    {
        public decimal Balance { get; set; }

      //  public int BeneficiaryId { get; set; }
        public int UserId { get; set; }
    }
}
