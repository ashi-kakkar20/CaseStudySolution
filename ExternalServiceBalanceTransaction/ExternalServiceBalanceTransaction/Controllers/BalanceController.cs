using ExternalServiceBalanceTransaction.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;


namespace ExternalServiceBalanceTransaction.Controllers
{
    public class BalanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BalanceController(AppDbContext context)
        {
            _context = context;

        }

        // GET: api/balance/{userId}
        [HttpGet]
        [Route("api/balance/{userId}")]
        public ActionResult<decimal> GetUserBalance(int userId)
        {
            try
            {
                var result = (from x in _context.BalanceInformation.AsEnumerable()
                              where x.UserId == userId // && x.BeneficiaryId== beneficiaryid
                              select x).FirstOrDefault();
                if (result == null)
                {
                    return 0;
                }
                else
                {
                    decimal totalBal = result.Balance;
                    return totalBal;
                }
            }

            catch(Exception ex)
            {
                return 0;
                throw ex;
            }
           
           
        }

        [HttpPut]
        [Route("api/updatebalance/{userId}")]
        public ActionResult UpdateUserBalance([FromBody] BalanceInformation request)
        {
            try
            {
                //var userfound = _context.BalanceInformation.Where(a => a.UserId == request.UserId && a.BeneficiaryId == request.BeneficiaryId).AsNoTracking().FirstOrDefault();

                var userfound = (from x in _context.BalanceInformation.AsEnumerable()
                              where x.UserId == request.UserId // && x.BeneficiaryId == request.BeneficiaryId
                                 select x).FirstOrDefault();

                userfound.Balance = request.Balance;

                _context.BalanceInformation.Update(userfound);

                _context.SaveChanges();


                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
                throw ex;
            }
        }

    }
}
