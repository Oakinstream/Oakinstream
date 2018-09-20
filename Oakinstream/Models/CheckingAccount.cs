using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class CheckingAccount
    {
        public int ID { get; set; }
        public int AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}