using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab4.Core
{
    public abstract class Account
    {
        private decimal _balance;
        private readonly string _owner;

        public string Owner { get { return _owner; } }

        public decimal Balance
        {
            get { return _balance; }
            protected set
            {
                if (value < -1_000_000_000m)
                    throw new ArgumentOutOfRangeException(nameof(value), "Баланс слишком мал");
                _balance = value;
            }
        }

        protected Account(string owner, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Владелец не может быть пустым",nameof(owner));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Начальный баланс не может быть отрицательным");

            _owner = owner;
            _balance = initialBalance;
        }

        public virtual void Deposit (decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");
            Balance += amount;
        }

        public abstract void Withdraw(decimal amount);
        public abstract void AccrueInterest();

        public override string ToString() => $"{GetType().Name}({Owner}): {Balance}";
    }

    public class DebitAccount : Account
    {
        public DebitAccount (string owner, decimal initialBalance = 0) : base(owner, initialBalance){}

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (amount > Balance)
                throw new InvalidOperationException("Недостаточно средств");
            Balance -= amount;
        }

        public override void AccrueInterest()
        {
            Balance += Balance * 0.01m;
        }

        public override string ToString() => $"{GetType().Name} ({Owner}): {Balance}";
    }

    public class CreditAccount : Account
    {
        private decimal _creditLimit;

        public decimal CreditLimit
        {
            get { return _creditLimit; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Кредитный лимит должен быть > 0");
                _creditLimit = value;
            }
        }

        public CreditAccount(string owner, decimal creditLimit, decimal initialBalance=0):base(owner, initialBalance)
        {
            CreditLimit = creditLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <=0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (Balance - amount < -CreditLimit)
                throw new InvalidOperationException("Превышен кредитный лимит");
            Balance -= amount;
        }

        public override void AccrueInterest()
        {
            if (Balance < 0)
                Balance -= -Balance * 0.20m;
        }

        public override string ToString() => $"{GetType().Name} ({Owner}): {Balance:C}, лимит: {CreditLimit:C}";
    }

    public class DepositAccount : Account
    {
        private decimal _interestRate;
        private int _termMonth;
        private int _monthPassed;

        public decimal InterestRate
        {
            get { return _interestRate; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Ставка должна быть > 0");
                _interestRate = value;
            }
        }

        public int TermMonths
        {
            get { return _termMonth; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Срок должен быть > 0");
                _termMonth=value;
            }
        }

        public DepositAccount(string owner, decimal initialBalance, decimal interestRate, int termMonths): base(owner, initialBalance)
        {
            InterestRate = interestRate;
            TermMonths = termMonths;
            _monthPassed = 0;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (_monthPassed < TermMonths)
                throw new InvalidOperationException("Снятие запрещено до окончания срока");
            if (amount > Balance)
                throw new InvalidOperationException("Недостаточно средств");
            Balance -= amount;
        }

        public override void AccrueInterest()
        {
            if (_monthPassed >= TermMonths) return;
            Balance += Balance * (InterestRate / 12);
            _monthPassed++;
        }

        public override string ToString() => $"{GetType().Name} ({Owner}): {Balance:C}, ставка: {InterestRate:P}, срок: {TermMonths} мес.";
    }

}
