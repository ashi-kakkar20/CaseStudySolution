using System.Linq;
using TopUp_Beneficiary.Interfaces;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public Users Authenticate(string username, string password)
        {
            //  return await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password);
            var result =  _context.Users
    .Where(u => u.Username == username && u.Password == password).SingleOrDefault();

            return result;
        }

        public int GetUserId(string username)
        {
            var detail = (from x in _context.Users.AsEnumerable()
                          where x.Username.ToString() == username
                          select x).FirstOrDefault();

            if (detail == null)
            {
                return 0;
            }
            else
            {

                int UserId = detail.Id;
                return UserId;
            }


        }

    }
}
