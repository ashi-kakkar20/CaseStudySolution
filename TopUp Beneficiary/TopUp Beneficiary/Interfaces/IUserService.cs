using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary.Interfaces
{
    public interface IUserService
    {
        Users Authenticate(string username, string password);

        int GetUserId(string username);
    }
}
