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

internal class Program
{
	private static void Main(string[] args)
	{
		
	}
}