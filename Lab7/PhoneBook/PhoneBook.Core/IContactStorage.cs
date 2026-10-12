using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace PhoneBook.Core
{
    public interface IContactStorage
    {
        void Save(IEnumerable<Contact> contacts);
        List<Contact> Load();
    }

    public class FileContactStorage: IContactStorage
    {
        private readonly string _path;
        public FileContactStorage(string path) => _path = path;

        public void Save(IEnumerable<Contact> contacts)
        {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(contacts);
        }

        public List<Contact> Load()
        {
            if (!File.Exists(_path)) return new List<Contact>();
            using var reader = new StreamReader(_path);
            using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            return csv.GetRecords<Contact>().ToList();
        }
    }
    public class MemoryContactStorage : IContactStorage
    {
        private List<Contact> _contacts = new();
        public void Save(IEnumerable<Contact> contacts) => _contacts = contacts.ToList();
        public List<Contact> Load() => _contacts.ToList();
    }
}
