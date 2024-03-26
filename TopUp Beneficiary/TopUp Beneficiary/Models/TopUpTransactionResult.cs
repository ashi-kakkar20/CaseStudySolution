namespace TopUp_Beneficiary.Models
{
    public class TopUpTransactionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal NewBalance { get; set; }
    }
}
