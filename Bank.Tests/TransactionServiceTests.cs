using Xunit;
using Microsoft.EntityFrameworkCore;
using Bank.Infrastructure.Data;
using Bank.Infrastructure.Services;
using Bank.Core.Entities;
using Bank.Core.DTOs;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bank.Tests;

public class TransactionServiceTests
{
    [Fact]
    public async Task TransferAsync_ShouldTransferMoney_WhenDataIsValid()
    {
      var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        using var context = new AppDbContext(options);
        
        var sender = new Account { Id = 1, Balance = 1000 };
        var receiver = new Account { Id = 2, Balance = 500 };
        context.Accounts.AddRange(sender, receiver);

        await context.SaveChangesAsync();

        var transactionService = new TransactionService(context);
        var transferDto = new TransferDto
        {
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            Amount = 200
        };


        await transactionService.TransferAsync(transferDto);

        var updatedSender = await context.Accounts.FindAsync(1);
        var updatedReceiver = await context.Accounts.FindAsync(2);
        Assert.NotNull(updatedSender);
        Assert.NotNull(updatedReceiver);

        Assert.Equal(800, updatedSender.Balance);
        Assert.Equal(700, updatedReceiver.Balance);

        Assert.Equal(1, await context.Transactions.CountAsync());
    }    

    [Fact]
    public async Task Transfer_ToNonExistentAccount_ShouldThrowException()
    {
      var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        using var context = new AppDbContext(options);
        
        var sender = new Account { Id = 1, Balance = 1000 };
        context.Accounts.Add(sender);


        await context.SaveChangesAsync();

        var transactionService = new TransactionService(context);
        var transferDto = new TransferDto
        {
            SenderAccountId = 1,
            ReceiverAccountId = 123,
            Amount = 200
        };

        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await transactionService.TransferAsync(transferDto)
        );
        Assert.Equal("Sender account not found.", exception.Message);
    }

}