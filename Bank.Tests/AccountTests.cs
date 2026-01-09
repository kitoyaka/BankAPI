using Xunit;
using Bank.Core.Entities;

namespace Bank.Tests;

public class AccountTests
{
    [Fact]
    public void Debit_WithValidAmount_ShouldReduceBalance()
    {
        var clientAccount = new Account { Balance = 1000 }; 

        clientAccount.Debit(200);

        Assert.Equal(800, clientAccount.Balance);
    }

    [Fact]
    public void Debit_InsufficientFunds_ShouldThrowException()
    {
        var clientAccount = new Account { Balance = 100 };

        var exception = Assert.Throws<Exception>(() => clientAccount.Debit(500));

        Assert.Equal("Insufficient funds.", exception.Message);
    }

    [Fact]
    public void Credit_WithValidAmount_ShouldIncreaseBalance()
    {
        var clientAccount = new Account { Balance = 100 };

        clientAccount.Credit(300);

        Assert.Equal(400, clientAccount.Balance);
    }

    [Fact]
    public void Credit_NegativeAmount_ShouldThrowException()
    {
        var userAccount = new Account { Balance = 500 };

        var exception = Assert.Throws<Exception>(() => userAccount.Credit(-100));

        Assert.Equal("Amount to credit must be greater than zero.", exception.Message);
    }
}
