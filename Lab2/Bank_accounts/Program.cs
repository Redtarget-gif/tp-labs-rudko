using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public abstract class Account
{
	private decimal _balance;
	private readonly string _owner;

	public string Owner
	{
		get { return _owner; }
	}

	public decimal Balance
	{
		get { return _balance; }
		protected set
		{
			if (value<-1_000_000_000m)
				throw new ArgumentOutOfRangeException(nameof(value));
			_balance = value;
		}
	}
	protected Account(string owner, decimal initialBalance = 0)
	{
		if (string.IsNullOrEmpty(owner))
			throw new ArgumentOutOfRangeException(nameof(initialBalance));
		_owner = owner;
		_balance = initialBalance;
	}
	public virtual void Deposit(decimal amount)
	{
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");
		Balance += amount;
	}

	public abstract void Withdraw(decimal amount);
	public abstract void AccrueInterest();
	public override string ToString()
	{
		return $"{GetType().Name} ({Owner}): {Balance:C}";
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
			Console.WriteLine($"{GetType().Name} Недостаточно средств");
		Balance -= amount;
    }

    public override void AccrueInterest()
    {
		Balance += Balance*0.1m;
    }

    public override string ToString()
    {
		return $"{GetType().Name} ({Owner}): {Balance:C}";
    }
}

public class CreditAccount: Account
{
	private decimal _creditLimit;
	public decimal CreditLimit
	{
		get { return _creditLimit; }
		private set
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException(nameof(value));
			_creditLimit = value;
		}
	}


	public CreditAccount(string owner, decimal creditLimit, decimal initialBalance = 0):base(owner, initialBalance)
	{
		CreditLimit = creditLimit;
	}

    public override void Withdraw(decimal amount)
    {
		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));
		if (Balance - amount < -CreditLimit)
			throw new InvalidOperationException("Превышен кредитный лимит");
		Balance -= amount;
    }

	public override void AccrueInterest()
	{
		if (Balance < 0)
			Balance-=Balance*0.20m;
	}
    public override string ToString()
    {
		return $"{GetType().Name} ({Owner}): {Balance:C}, кредитный лимит: {CreditLimit:C}";
    }
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
				throw new ArgumentOutOfRangeException(nameof(value));
			_interestRate = value;
		}
	}

	public int TermMonth
	{
		get { return _termMonth; }
		private set
		{
			if (value <= 0)
				throw new ArgumentOutOfRangeException(nameof(value));
			_termMonth = value;
		}
	}

	public DepositAccount(string owner, decimal initialBalance, decimal interestRate, int termMonths) : base(owner, initialBalance)
    {
		InterestRate = interestRate;
		TermMonth = termMonths;
		_monthPassed = 0;
    }

    public override void Withdraw(decimal amount)
    {

		if (amount <= 0)
			throw new ArgumentOutOfRangeException(nameof(amount));
        if (_monthPassed < TermMonth)
			Console.WriteLine($"{GetType().Name} Снятие запрещено до окончания срока ({TermMonth - _monthPassed} мес. осталось)");
;
		if (amount > Balance)
			Console.WriteLine($"{GetType().Name} Недостаточно средств");
		Balance -= amount;
    }
    public override void AccrueInterest()
    {
        if (_monthPassed >= TermMonth)
			return;
		decimal monthlyRate = InterestRate / 12;
		Balance += Balance * monthlyRate;
		_monthPassed++;
    }
    public override string ToString()
    {
		return $"{GetType().Name}({Owner}): {Balance:C}, ставка: {InterestRate:P}, срок: {TermMonth} мес.";
    }
}

class Program
{
	 static void Main()

	{
        static void ShowOperation(Account acc, string operation)
        {
            Console.WriteLine($"{acc.GetType().Name} {acc.Owner}: {operation} => {acc.Balance:C}");
        }
        Console.OutputEncoding = System.Text.Encoding.UTF8;

		var accounts = new List<Account>
		{
			new DebitAccount("Иванов",1000),
			new DebitAccount("Петрова",2000),
			new CreditAccount("Петров",5000,1000),
			new CreditAccount("Сидорова", 3000,500),
			new DepositAccount("Сидоров", 1000,0.10m,3)
		};

		foreach (var acc in accounts)
            Console.WriteLine(acc);

        Console.WriteLine("Операции");

        accounts[0].Deposit(500);
        ShowOperation(accounts[0], "пополнение 500,00 ₽");
        accounts[0].Withdraw(200);
        ShowOperation(accounts[0], "снятие 200,00 ₽");
        accounts[0].AccrueInterest();
        ShowOperation(accounts[0], "проценты");

        accounts[1].Deposit(300);
        ShowOperation(accounts[1], "пополнение 300,00 ₽");

        accounts[2].Withdraw(3000);
        ShowOperation(accounts[2], "снятие 3000,00 ₽");
        accounts[2].AccrueInterest();
        ShowOperation(accounts[2], "проценты на долг");

        accounts[3].Withdraw(1000);
        ShowOperation(accounts[3], "снятие 1000,00 ₽");

        for (int i = 0; i < 3; i++)
        {
            accounts[4].AccrueInterest();
            ShowOperation(accounts[4], "проценты за месяц");
        }
        accounts[4].Withdraw(1000);
        ShowOperation(accounts[4], "снятие 1000,00 ₽");

        Console.WriteLine("\n=== Итоги ===");
        foreach (var acc in accounts)
            Console.WriteLine(acc);
    }
}