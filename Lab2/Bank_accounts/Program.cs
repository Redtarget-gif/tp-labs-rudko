using System;
using System.Collections.Generic;

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
internal class Program
{
    private static void Main(string[] args)
    {
        
    }
}