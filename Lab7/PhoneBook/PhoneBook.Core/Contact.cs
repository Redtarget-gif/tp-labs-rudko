using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Core
{
    public class Contact
    {
        public string Name { get; set; } = "";
        public string Phone { get; set; }="";

        public override string ToString() => $"{Name}: {Phone}";
        
    }
}
