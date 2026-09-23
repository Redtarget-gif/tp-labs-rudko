using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public abstract class Account
{
	public string Owner { get; }
	public decimal Balance { get; protected set; }

	protected Account(string owner, decimal initialBalance = 0)
	{
		Owner = owner;
		Balance = initialBalance;
	}
	public virtual void Deposit(decimal amount)
	{
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");
		Balance += amount;
		Console.WriteLine($"{GetType().Name} пополнение на {amount:C}. Баланс: {Balance:C}");
	}

	public abstract void Withdraw(decimal amount);
	public abstract void AccrueInterest();
	public override string ToString()
	{
		string typeName = GetType().Name;
		string formattedBalance = string.Format("{0:C}", Balance);
		return typeName + " (" + Owner + "): " + formattedBalance;
	}
}

public class DebitAccount:Account
{
	public DebitAccount(string owner, decimal initialBalance = 0): base(owner, initialBalance) { }

    public override void Withdraw(decimal amount)
    {
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));
		if (amount > Balance)
		{
			Console.WriteLine($"{GetType().Name} Недостаточно средств. Баланс: {Balance:C}");
			return;
		}

		Balance -= amount;
		Console.WriteLine($"{GetType().Name} Снятие {amount:C}. Баланс: {Balance:C}");
    }

    public override void AccrueInterest()
    {
		decimal interest = Balance * 0.01m;
		Balance += interest;
		Console.WriteLine($"{GetType().Name} Начислены проценнты: {interest:C}. Баланс: {Balance:C}");
    }
}

public class CreditAccount: Account
{
	public decimal CreditLimit { get; }

	public CreditAccount(string owner, decimal creditLimit, decimal initialBalance = 0):base(owner, initialBalance)
	{
		if (creditLimit <= 0)
			throw new ArgumentOutOfRangeException(nameof(creditLimit));
		CreditLimit = creditLimit;
	}

    public override void Withdraw(decimal amount)
    {
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));
		if (Balance - amount < -CreditLimit)
		{
			Console.WriteLine($"{GetType().Name} Превышен кредитный лимит. Доступно: {Balance+CreditLimit:C}");
			return;
		}

		Balance -= amount;
		Console.WriteLine($"{GetType().Name} Снятие {amount:C}. Баланс: {Balance:C}");
    }

	public override void AccrueInterest()
	{
		if (Balance < 0)
		{
			decimal interest = -Balance * 0.20m;
			Balance-=interest;
			Console.WriteLine($"{GetType().Name} Начислены проценты за долг: {interest:C}. Баланс: {Balance:C}");
		}
		else
		{
			Console.WriteLine($"{GetType().Name} Долга нет, проценты не начисляются.");
		}
	}
}

public class DepositAccount : Account
{
	public decimal InterestRate { get; }
	public int TermMonth { get; }
	private int _monthPassed;

	public DepositAccount(string owner, decimal initialBalance, decimal interestRate, int termMonths) : base(owner, initialBalance)
    {
		if (interestRate <= 0)
			throw new ArgumentOutOfRangeException(nameof(interestRate));
		if (termMonths <= 0) throw new ArgumentOutOfRangeException(nameof(termMonths));
		InterestRate = interestRate;
		TermMonth = termMonths;
		_monthPassed = 0;
    }

    public override void Withdraw(decimal amount)
    {
        if (_monthPassed < TermMonth)
		{
			Console.WriteLine($"{GetType().Name} Снятие запрещено до окончания срока ({TermMonth - _monthPassed} мес. осталось)");
			return;
		}
;
		if (amount > Balance)
		{
			Console.WriteLine($"{GetType().Name} Недостаточно средств.");
			return;
		}
		Balance -= amount;
		Console.WriteLine($"{GetType().Name} Снятие {amount:C}. Баланс: {Balance:C}");
    }
    public override void AccrueInterest()
    {
        if (_monthPassed >= TermMonth)
		{
			Console.WriteLine($"{GetType().Name} Срок депозита истёк, проценты больше не начисляются.");
			return;
		}

		decimal monthlyRate = InterestRate / 12;
		decimal interest = Balance * monthlyRate;
		Balance += interest;
		_monthPassed++;
		Console.WriteLine($"{GetType().Name} Начислены проценты за месяц: {interest:C}. Баланс: {Balance:C}");
    }
}

class Program
{
	 static void Main()
	{
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Банковские счета");

		var accounts = new List<Account>
		{
			new DebitAccount("Иванов",1000),
			new CreditAccount("Петров",5000,1000),
			new DepositAccount("Сидоров", 1000,0.10m,3)
		};

		foreach (var acc in accounts)
            Console.WriteLine(acc);

        Console.WriteLine("Операции");

		accounts[0].Deposit(500);
		accounts[0].Withdraw(200);
		accounts[0].AccrueInterest();

		Console.WriteLine();

		accounts[1].Withdraw(3000);
		accounts[1].AccrueInterest();

		Console.WriteLine();

		accounts[2].AccrueInterest();
		accounts[2].AccrueInterest();
		accounts[2].AccrueInterest();
		accounts[2].Withdraw(1000);

		Console.WriteLine("Итоги");
		foreach (var acc in accounts)
			Console.WriteLine(acc);
    }
}