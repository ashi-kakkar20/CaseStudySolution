using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TopUp_Beneficiary;
using TopUp_Beneficiary.Models;
using TopUp_Beneficiary.Services;
using Xunit;

namespace TopUp_Beneficiary_Test
{
    public class TopUpServiceTests
    {
      

    [Fact]
    public async Task GetTopUpBeneficiaries_ReturnsBeneficiariesForUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database name
            .Options;

         
        // Seed the in-memory database with test data
        using (var context = new AppDbContext(options))
        {
            context.TopUpBeneficiary.AddRange(new List<TopUpBeneficiary>
                {
                new TopUpBeneficiary { TopUpBeneficiaryId= 1, BeneficiaryName= "Test User 1",Nickname= "TestUser_1",UserId=1 } ,
                new TopUpBeneficiary { TopUpBeneficiaryId= 3, BeneficiaryName= "Test User 3",Nickname= "TestUser_3",UserId=1 },
                new TopUpBeneficiary { TopUpBeneficiaryId= 4, BeneficiaryName= "Test User 4",Nickname= "TestUser_4",UserId=1 },
                new TopUpBeneficiary { TopUpBeneficiaryId= 5, BeneficiaryName= "Test User 5",Nickname= "TestUser_5",UserId=1 },
                new TopUpBeneficiary { TopUpBeneficiaryId= 7, BeneficiaryName= "Test User 6",Nickname= "TestUser_6",UserId=1 }
                });
            context.SaveChanges();
        }

        // Act
        List<TopUpBeneficiary> result;
        using (var context = new AppDbContext(options))
        {
            var service = new TopUpService(context);
            result = service.GetTopUpBeneficiaries(1).Result;
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
        Assert.All(result, b => Assert.Equal(1, b.UserId));
    }

    [Fact]
    public void GetTopUpOptions_ReturnsTopUpOptions()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database name
            .Options;

        // Seed the in-memory database with test data
        using (var context = new AppDbContext(options))
        {
            context.TopUpOption.AddRange(new List<TopUpOption>
                {
                    new TopUpOption { Amount = 5 },
                    new TopUpOption { Amount = 10 },
                    new TopUpOption { Amount = 20 },
                    new TopUpOption { Amount = 30 },
                    new TopUpOption { Amount = 50 },
                    new TopUpOption { Amount = 75 },
                    new TopUpOption { Amount =  100 },
                });
            context.SaveChanges();
        }

        // Act
        List<string> result;
        using (var context = new AppDbContext(options))
        {
            var service = new TopUpService(context);
            result = service.GetTopUpOptions().Result;
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count);
        Assert.Contains("AED 5", result);
        Assert.Contains("AED 10", result);
        Assert.Contains("AED 20", result);
        Assert.Contains("AED 30", result);
        Assert.Contains("AED 50", result);
        Assert.Contains("AED 75", result);
        Assert.Contains("AED 100", result);

    }

    [Fact]
    public async Task AddTopUpBeneficiary_LongNickname()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new AppDbContext(options))
        {
            var topUpApiService = new TopUpService(context);

            var beneficiaryName = "Test User 1";
            var nickname = "ThisIsAVeryLongNicknameThatExceedsTwentyCharacters";
            var userId = 1;
            var BeneficiaryPhoneNumber = "0544678312";

            // Act
            var result = await topUpApiService.AddTopUpBeneficiary(beneficiaryName, nickname, userId, BeneficiaryPhoneNumber);

            // Assert
            Assert.Equal("Please enter a shorter beneficiary nickname (maxlength of 20 characters)!", result);
        }
    }


    [Fact]
    public async Task AddTopUpBeneficiary_MaximumBeneficiariesReached()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;


        using (var context = new AppDbContext(options))
        {

            for (int i = 0; i < 5; i++)
            {
                context.TopUpBeneficiary.Add(new TopUpBeneficiary { UserId = 1 });
            }
            await context.SaveChangesAsync();
        }


        using (var context = new AppDbContext(options))
        {
            var topUpApiService = new TopUpService(context);

            var beneficiaryName = "Test User 1";
            var nickname = "TestUser_1";
            var userId = 1;
            var BeneficiaryPhoneNumber = "0544678312";

            // Act
            var result = await topUpApiService.AddTopUpBeneficiary(beneficiaryName, nickname, userId, BeneficiaryPhoneNumber);

            // Assert
            Assert.Equal("User already has 5 beneficiaries", result);
        }
    }


    [Fact]
    public async Task AddTopUpBeneficiary_Success()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;


        using (var context = new AppDbContext(options))
        {
            var topUpApiService = new TopUpService(context);

            var beneficiaryName = "Test User 1";
            var nickname = "Test User_1";
            var userId = 1;
            var BeneficiaryPhoneNumber = "0544731784";

            // Act
            var result = await topUpApiService.AddTopUpBeneficiary(beneficiaryName, nickname, userId, BeneficiaryPhoneNumber);

            // Assert
            Assert.Equal("Beneficiary Test User 1 added successfully", result);
        }
    }

    [Fact]
    public void GetSumOfTransactionhistoryForABeneficiaryThisMonth_ReturnsTotalAmount()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;


        using (var context = new AppDbContext(options))
        {

            context.TopUpTransaction.AddRange(new[]
            {
                    new TopUpTransaction { TubID = 1, UserId = 1, TransactionDate = DateTime.Now, Amount = 50 },
                    new TopUpTransaction { TubID = 1, UserId = 1, TransactionDate = DateTime.Now, Amount = 100 },
                    new TopUpTransaction { TubID = 2, UserId = 1, TransactionDate = DateTime.Now.AddMonths(-1), Amount = 150 }
                });
            context.SaveChanges();

            var topUpApiService = new TopUpService(context);

           // var beneficiaryId = 1;
            var userId = 1;

            // Act
            var totalAmount = topUpApiService.GetSumOfTransactionhistoryForAUserThisMonth(userId);

            // Assert
            Assert.Equal(150, totalAmount);
        }
    }

    [Fact]
    public async Task AddTopUp_SuccessfulTransaction()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;


        using (var context = new AppDbContext(options))
        {

            context.Users.Add(new Users { Id =2, IsVerified = true }); // Assume user is verified
            context.TopUpBeneficiary.Add(new TopUpBeneficiary { TopUpBeneficiaryId = 1, UserId = 2 ,BeneficiaryName="TestUser1",BeneficiaryPhoneNumber= "0556693519" }); // Assuming beneficiary exists
            await context.SaveChangesAsync();

            var topUpApiService = new TopUpService(context);

            var userId = 2;
            var beneficiaryPhoneNumber = "0556693519";
            var amount = 50m;

            // Mocking the external HTTP service call
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"balance\": 500}") });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await topUpApiService.AddTopUp(userId, beneficiaryPhoneNumber, amount);


            Assert.Equal(450m, result.NewBalance);
        }
    }
}
  
}



   

