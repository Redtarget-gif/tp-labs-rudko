using System;
using System.Collections.Generic;
using System.Text;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests
{
    public class MyBankAccountsTests
    {
        [Fact]
        public void DebitAccount_Deposit_IncreaseBalance()
        {
            var acc = new DebitAccount("Иванов", 1000);
            acc.Deposit(500);
            Assert.Equal(1500,acc.Balance);
        }

        [Fact]
        public void DebitAccount_Withdraw_DecreasesBalance()
        {
            var acc = new DebitAccount("Иванов", 1000);
            acc.Withdraw(300);
            Assert.Equal(700, acc.Balance);
        }

        [Fact]
        public void DebitAccount_WithdrawMoreThanBalance_Throws()
        {
            var acc = new DebitAccount("Иванов", 1000);
            Assert.Throws<InvalidOperationException>(() => acc.Withdraw(2000));
        }

        [Fact]
        public void DebitAccount_NegativeDeposit_Throws()
        {
            var acc = new DebitAccount("Иванов", 1000);
            Assert.Throws<ArgumentOutOfRangeException>(() => acc.Deposit(-100));
        }
        [Fact]
        public void DebitAccount_AccrueInterest_IncreasesBalance()
        {
            var acc = new DebitAccount("Иванов", 1000);
            acc.AccrueInterest();
            Assert.Equal(1010, acc.Balance);
        }

        [Fact]
        public void CreditAccount_WithdrawWithinLimit_Works()
        {
            var acc = new CreditAccount("Петров", 5000, 1000);
            acc.Withdraw(3000);
            Assert.Equal(-2000, acc.Balance); ;
        }

        [Fact]
        public void CreditAccount_WithdrawBeyondLimit_Throws()
        {
            var acc = new CreditAccount("Петров", 5000, 1000);
            Assert.Throws<InvalidOperationException>(() => acc.Withdraw(7000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void CreditAccount_InvalidLimit_Throws(decimal limit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CreditAccount("Петров", limit));
        }

        [Fact]
        public void CreditAccount_AccrueInterestOnDebt_IncreaseseDebt()
        {
            var acc = new CreditAccount("Петров", 5000, 1000);
            acc.Withdraw(3000);
            acc.AccrueInterest();
            Assert.Equal(-2400, acc.Balance);
        }

        [Fact]
        public void DepositAccount_WithdrawBeforeTerm_Throws()
        {
            var acc = new DepositAccount("Сидоров", 10000, 0.10m, 3);
            Assert.Throws<InvalidOperationException>(() => acc.Withdraw(1000));
        }

        [Fact]
        public void DepositAccount_AccrueInterest_IncreasesBalance()
        {
            var acc = new DepositAccount("Сидоров", 10000, 0.10m, 3);
            acc.AccrueInterest();
            Assert.True(acc.Balance > 10000);
        }
        [Fact]
        public void DepositAccount_WithdrawAfterTerm_Works()
        {
            var acc = new DepositAccount("Сидоров", 10000, 0.10m, 3);
            for (int i = 0; i < 3; i++) acc.AccrueInterest();
            acc.Withdraw(1000);
            Assert.True(acc.Balance < 11000);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DepositAccount_InvalidTerm_Throws(int term)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DepositAccount("Сидоров", 10000, 0.10m, term));
        }
        [Fact]
        public void Account_EmptyOwner_Throws()
        {
            Assert.Throws<ArgumentException>(()=>new DebitAccount("",1000));
        }
    }

}
