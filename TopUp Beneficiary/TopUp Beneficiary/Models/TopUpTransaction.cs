using System;

namespace TopUp_Beneficiary.Models
{
    public class TopUpTransaction
    {
        public int TopUpTransactionId { get; set; }
        public int TubID { get; set; }
         
        public int UserId { get; set; }
       
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
