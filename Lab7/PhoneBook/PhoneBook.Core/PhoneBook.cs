using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Core
{
    public class PhoneBookService
    {
        private readonly List<Contact> _contacts = new();
        private readonly IContactStorage _storage;

        public PhoneBookService(IContactStorage storage) => _storage = storage;

        public IReadOnlyList<Contact> Contacts => _contacts;

        public void Add(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Name))
                throw new ArgumentException("Имя не может быть пустым");
            if (string.IsNullOrWhiteSpace(contact.Phone))
                throw new ArgumentException("Телефон не может быть пустым");
            _contacts.Add(contact);
        }

        public void Remove(string name)
        {
            _contacts.RemoveAll(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Contact> Search(string query)
        {
            return _contacts.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || c.Phone.Contains(query)).ToList();
        }

        public List<Contact> SortByName() => _contacts.OrderBy(c => c.Name).ToList();

        public List<Contact> SortByPhone() => _contacts.OrderBy(c => c.Phone).ToList();

        public void Save() => _storage.Save(_contacts);

        public void Load()
        {
            _contacts.Clear();
            _contacts.AddRange(_storage.Load());
        }
    }
}
