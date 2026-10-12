using System;
using System.Collections.Generic;
using System.Text;
using PhoneBook.Core;
using Xunit;

    public class PhoneBookTests
    {
    private PhoneBook.Core.PhoneBookService CreateBook() => new PhoneBook.Core.PhoneBookService(new MemoryContactStorage());

    [Fact]
    public void Add_ValidContact_IncreasesCount()
    {
        var book = CreateBook();
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        Assert.Single(book.Contacts);
    }

    [Fact]

    public void Add_EmptyName_Throws()
    {
        var book = CreateBook();
        Assert.Throws<ArgumentException>(() => book.Add(new Contact { Name = "", Phone = "123" }));
    }
    [Fact]
    public void Add_EmptyPhone_Throws()
    {
        var book = CreateBook();
        Assert.Throws<ArgumentException>(() => book.Add(new Contact { Name = "Иван", Phone = "" }));
    }
    [Fact]
    public void Remove_ExistingContact_Removes()
    {
        var book = CreateBook();
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        book.Remove("Иван");
        Assert.Empty(book.Contacts);
    }

    [Fact]
    public void Search_ByName_Finds()
    {
        var book=CreateBook();
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        book.Add(new Contact { Name = "Пётр", Phone = "456" });
        var results = book.Search("Иван");
        Assert.Single(results);
    }

    [Fact]
    public void Search_ByPhone_Finds()
    {
        var book = CreateBook();
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        var results = book.Search("123");
        Assert.Single(results);
    }

    [Fact]
    public void SortByName_ReturnsSorted()
    {
        var book = CreateBook();
        book.Add(new Contact { Name = "Пётр", Phone = "456" });
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        var sorted = book.SortByName();
        Assert.Equal("Иван", sorted[0].Name);
    }
    [Fact]
    public  void SortByPhone_ReturnsSorted()
    {
        var book = CreateBook();
        book.Add(new Contact { Name = "Иван", Phone = "456" });
        book.Add(new Contact { Name = "Пётр", Phone = "123" });
        var sorted = book.SortByPhone();
        Assert.Equal("123", sorted[0].Phone);
    }
    [Fact]
    public void SaveAndLoad_RestoresContacts()
    {
        var storage = new MemoryContactStorage();
        var book = new PhoneBook.Core.PhoneBookService(storage);
        book.Add(new Contact { Name = "Иван", Phone = "123" });
        book.Save();

        var book2=new PhoneBook.Core.PhoneBookService(storage);
        book2.Load();
        Assert.Single(book2.Contacts);
    }
    }
