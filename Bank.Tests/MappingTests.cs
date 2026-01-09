using Xunit;
using Bank.Core.Entities;
using Bank.Core.DTOs;
using Bank.Infrastructure.Extensions;

namespace Bank.Tests;

public class MappingTests
{
    [Fact]
    public void ToTransactionEntity_ShouldMapAllPropertiesCorrectly()
    {
        var transferDto = new TransferDto
        {
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            Amount = 500
        };
        var transaction = transferDto.ToTransactionEntity();

        Assert.Equal(transferDto.SenderAccountId, transaction.SenderAccountId);
        Assert.Equal(transferDto.ReceiverAccountId, transaction.ReceiverAccountId);
        Assert.Equal(transferDto.Amount, transaction.Amount);
        Assert.NotEqual(DateTime.MinValue, transaction.TransactionDate);

    }

    [Fact]
    public void ToTransactionDto_ShouldMapAllPropertiesCorrectly()
    {
        var transaction = new Transaction
        {
            Id = 10,
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            Amount = 500,
            TransactionDate = DateTime.UtcNow
        };

        transaction.ToTransactionDto();

        Assert.Equal(transaction.Id, transaction.ToTransactionDto().TransactionId);
        Assert.Equal(transaction.SenderAccountId, transaction.ToTransactionDto().SenderAccountId);
        Assert.Equal(transaction.ReceiverAccountId, transaction.ToTransactionDto().ReceiverAccountId);
        Assert.Equal(transaction.Amount, transaction.ToTransactionDto().Amount);
        Assert.Equal(transaction.TransactionDate, transaction.ToTransactionDto().Date);
    }

}