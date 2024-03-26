using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TopUp_Beneficiary.Interfaces;

namespace TopUp_Beneficiary.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;
        private readonly IUserService _userService;


        public TopUpController(ITopUpService topUpService, IUserService userService)
        {
            _topUpService = topUpService;
            _userService = userService;

        }

        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetTopUpBeneficiaries()
        {
            try
            {
                int userId = _userService.GetUserId(User.Identity.Name.ToString()); // Retrieve user ID from JWT token
                var beneficiaries = await _topUpService.GetTopUpBeneficiaries(userId);
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during GetTopUpBeneficiaries - " + ex.Message);

            }
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetTopUpOptions()
        {
            try
            {
                var options = await _topUpService.GetTopUpOptions();
                return Ok(options);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during GetTopUpOptions - " + ex.Message);

            }
        }

        [HttpPost("topupbeneficiary")]
        public async Task<IActionResult> TopUpBeneficiary(string Beneficiaryname,string nickname,string BeneficiaryPhoneNumber)
        {
            try
            {
              
                int userId = _userService.GetUserId(User.Identity.Name.ToString());
                string result = await _topUpService.AddTopUpBeneficiary(Beneficiaryname,nickname, userId,BeneficiaryPhoneNumber);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during TopUpBeneficiary - " + ex.Message);

            }
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp(TopUpRequest request)
        {
            try
            {
               
                int userId = _userService.GetUserId(User.Identity.Name.ToString());
                var result = await _topUpService.AddTopUp(userId, request.BeneficiaryPhoneNumber, request.Amount);

                if (result.Success) 
                { 
                    return Ok(result);
                }
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during TopUp - " + ex.Message);

            }
        }

        [HttpGet("GetTransactionhistoryThisMonth")]
        public async Task<IActionResult> GetTransactionhistoryThisMonth(int userid)
        {
            try
            {
                int userId = _userService.GetUserId(User.Identity.Name.ToString()); 
                var beneficiaries = await _topUpService.GetTransactionhistoryThisMonth(userId);
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during GetTransactionhistoryThisMonth - " + ex.Message);

            }
        }

        [HttpGet("GetSumOfTransactionhistoryForUserThisMonth")]
        public async Task<IActionResult> GetSumOfTransactionhistoryForUserThisMonth(int userId)
        {
            try
            {
                 userId = _userService.GetUserId(User.Identity.Name.ToString()); // Retrieve user ID from JWT token
                var beneficiaries =  _topUpService.GetSumOfTransactionhistoryForAUserThisMonth( userId);
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during GetSumOfTransactionhistoryForUserThisMonth - " + ex.Message);

            }
        }
    }

    public class TopUpRequest
    {
        public string BeneficiaryPhoneNumber { get; set; }
        public decimal Amount { get; set; }
    }
}