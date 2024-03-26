using System.Collections.Generic;
using System.Threading.Tasks;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary.Interfaces
{
    public interface ITopUpService
    {
        Task<List<TopUpBeneficiary>> GetTopUpBeneficiaries(int userId);
        Task<List<string>> GetTopUpOptions();
        Task<TopUpTransactionResult> AddTopUp(int userId, string beneficiaryphonenumber, decimal amount);
        Task<string> AddTopUpBeneficiary(string beneficiaryname, string nickname, int userid, string BeneficiaryPhoneNumber);
        Task<List<TopUpTransaction>> GetTransactionhistoryThisMonth(int userid);
        decimal GetSumOfTransactionhistoryForAUserThisMonth(int userid);
    }
}
