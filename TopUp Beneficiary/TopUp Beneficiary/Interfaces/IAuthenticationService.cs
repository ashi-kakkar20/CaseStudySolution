using Microsoft.AspNetCore.Mvc;

namespace TopUp_Beneficiary.Interfaces
{
        public interface IAuthenticationService
        {
            string GenerateJwtToken(string userId);
        }
    
}
