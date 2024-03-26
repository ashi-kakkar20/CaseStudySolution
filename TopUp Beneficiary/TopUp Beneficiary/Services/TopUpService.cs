using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TopUp_Beneficiary.Interfaces;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary.Services
{
    public class TopUpService : ITopUpService
    {
        public readonly AppDbContext _context;


        public TopUpService(AppDbContext context)
        {
            _context = context;

        }

        public TopUpService()
        {
        }

        public Task<List<TopUpBeneficiary>> GetTopUpBeneficiaries(int userId)
        {
            try
            {
                var result = _context.TopUpBeneficiary
         .Where(u => u.UserId == userId)
         .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetTopUpOptions()
        {

            try
            {
                var topUpOptions = await _context.TopUpOption.ToListAsync();
                List<string> stringstopUpOptions = new List<string>();
                for (int i = 0; i < topUpOptions.Count; i++)
                {
                    stringstopUpOptions.Add("AED " + topUpOptions[i].Amount.ToString());
                }


                return stringstopUpOptions;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> AddTopUpBeneficiary(string beneficiaryname, string nickname, int userid, string beneficiaryPhoneNumber)
        {

            try
            {
                var lstbeneficiaries = await GetTopUpBeneficiaries(userid);
                var beneficiaryCount = lstbeneficiaries.Count(b => b.UserId == userid);


                if (beneficiaryCount >= 5)
                {

                    return "User already has 5 beneficiaries"; ;
                }

                else
                {
                    if (nickname.Length > 20)
                    {
                        return "Please enter a shorter beneficiary nickname (maxlength of 20 characters)!";
                    }
                    var transaction = new TopUpBeneficiary
                    {

                        Nickname = nickname,
                        UserId = userid,
                        BeneficiaryName = beneficiaryname,
                        BeneficiaryPhoneNumber = beneficiaryPhoneNumber
                    };

                    _context.TopUpBeneficiary.Add(transaction);

                    await _context.SaveChangesAsync();
                    return "Beneficiary " + beneficiaryname + " added successfully";
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TopUpTransactionResult> AddTopUp(int userId, string beneficiaryPhoneNumber, decimal amount)
        {

            try
            {

                int beneficiaryid = 0;

                var resbeneficiaryId = (from x in _context.TopUpBeneficiary.AsEnumerable()
                                        where x.UserId == userId && x.BeneficiaryPhoneNumber == beneficiaryPhoneNumber
                                        select x).FirstOrDefault();

                if (resbeneficiaryId.ToString() == null)
                {
                    return new TopUpTransactionResult
                    {
                        Success = false,
                        Message = "Beneficiary with the Phone Number " + beneficiaryPhoneNumber + "not found for this user"
                    };
                }


                else
                {
                    beneficiaryid = resbeneficiaryId.TopUpBeneficiaryId;
                }

                string ExternalHttpServiceUrl = "https://localhost:44398";
                List<TopUpBeneficiary> lstbeneficiaries = await GetTopUpBeneficiaries(userId);
                var beneficiaryCount = lstbeneficiaries.Count(b => b.UserId == userId);
                decimal userBalance = 0;

                TopUpTransactionResult result = new TopUpTransactionResult();

                var detail = (from x in _context.Users.AsEnumerable()
                              where x.Id == userId
                              select x).FirstOrDefault();

                bool UserVerificationStatus = detail.IsVerified;


                // Retrieve user balance from external HTTP service
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ExternalHttpServiceUrl); /*https://localhost:44398*/
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync("api/balance/" + userId); //" +beneficiaryId+"

                    if (response.IsSuccessStatusCode)
                    {
                        var res = response.Content.ReadAsStringAsync();
                        userBalance = Convert.ToDecimal(res.Result);
                    }
                }

                // Check if user has sufficient balance for the top-up
                if (userBalance.ToString() == null || userBalance < amount + 1)
                {
                    return new TopUpTransactionResult
                    {
                        Success = false,
                        Message = "Insufficient balance for top-up."
                    };
                }

                //Retrieve beneficiary details
                var beneficiary = await _context.TopUpBeneficiary
                   // .Include(b => b.User)
                   .FirstOrDefaultAsync(b => b.TopUpBeneficiaryId == beneficiaryid && b.UserId == userId);

                if (beneficiary == null)
                {
                    return new TopUpTransactionResult
                    {
                        Success = false,
                        Message = "Beneficiary not found."
                    };
                }

                else
                {
                    var topUpOptions = await _context.TopUpOption.ToListAsync();

                    string[] amounts = new string[7];
                    for (int i = 0; i < topUpOptions.Count(); i++)
                    {
                        amounts[i] = topUpOptions[i].Amount.ToString();
                    }
                    bool AmountWronglyEntered = !(Array.Exists(amounts, element => element == amount.ToString()));
                    if (AmountWronglyEntered == true)
                    {
                        return new TopUpTransactionResult
                        {
                            Success = false,
                            Message = "Amount entered is Incorrect. Please use only below TopUp Amounts.(" + "AED 5, AED 10, AED 20, AED 30,AED 50, AED 75, AED 100)",
                            NewBalance = userBalance
                        };
                    }

                }

                // Check if user has exceeded monthly top-up limit
                decimal totalMonthlyTopUp = await _context.TopUpTransaction
                    .Where(t => t.UserId == userId && t.TransactionDate.Month == DateTime.UtcNow.Month)
                    .SumAsync(t => t.Amount);

                // Check if total top-up amount for all beneficiaries exceeds AED 3,000 per month
                if (totalMonthlyTopUp + amount > 3000)
                    return new TopUpTransactionResult
                    {
                        Success = false,
                        Message = "Total Amount exceeded for all beneficiaries- Topup not possible",
                        NewBalance = userBalance
                    };

                decimal maxTopUpAmountPerBeneficiary = UserVerificationStatus ? 500 : 1000;
                var totalTopUpForBeneficiaryThisMonth = _context.TopUpTransaction
                    .Where(t => t.TubID == beneficiaryid && t.TransactionDate.Month == DateTime.Now.Month && t.UserId == userId)
                    .Sum(t => t.Amount);

                if (totalTopUpForBeneficiaryThisMonth + amount > maxTopUpAmountPerBeneficiary)

                {
                    return new TopUpTransactionResult
                    {
                        Success = false,
                        Message = $"Monthly top-up limit exceeded. Monthly limit: {totalTopUpForBeneficiaryThisMonth}.",
                        NewBalance = userBalance
                    };
                }


                // Proceed with top-up
                //beneficiary.Balance += amount;
                // _dbContext.SaveChanges();

                // Update user balance
                userBalance -= amount + 1; //include charge of AED1


                // Perform top-up transaction
                var transaction = new TopUpTransaction
                {
                    TubID = beneficiaryid,
                    UserId = userId,
                    Amount = amount,
                    TransactionDate = DateTime.UtcNow
                };

                _context.TopUpTransaction.Add(transaction);

                await _context.SaveChangesAsync();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ExternalHttpServiceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    BalanceInformation biObj = new BalanceInformation();
                    biObj.UserId = userId;
                    // biObj.BeneficiaryId = beneficiaryId;
                    biObj.Balance = userBalance;

                    var myContent = JsonConvert.SerializeObject(biObj);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.PutAsync("api/updatebalance/" + userId, byteContent); //" +beneficiaryId+"

                }


                return new TopUpTransactionResult
                {
                    Success = true,
                    Message = "Top-up successful.",
                    NewBalance = userBalance
                };
            }

            catch (Exception ex) { throw ex; }
        }

        public Task<List<TopUpTransaction>> GetTransactionhistoryThisMonth( int userId)
        {
            try
            {
                var result = _context.TopUpTransaction
         .Where(t=> t.TransactionDate.Month == DateTime.Now.Month && t.UserId == userId)
         .ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal GetSumOfTransactionhistoryForAUserThisMonth(int userId)
        {
            try
            {

                var totalTopUpForUserThisMonth = _context.TopUpTransaction
                    .Where(t => t.TransactionDate.Month == DateTime.Now.Month && t.UserId == userId)
                    .Sum(t => t.Amount);

                return totalTopUpForUserThisMonth;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

