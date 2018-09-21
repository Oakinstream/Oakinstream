using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oakinstream.Models;

namespace Oakinstream.Services
{
    public class CheckingAccountService
    {
        private ApplicationDbContext db;

        public CheckingAccountService(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public void CreateCheckingAccount(string firstName, string lastName, string userId)
        {
            var accountNumber = (123456 + db.CheckingAccounts.Count()).ToString().PadLeft(10, '0');
            var checkingAccount = new CheckingAccount { FirstName = firstName, LastName = lastName, AccountNumber = accountNumber, ApplicationUserId = userId };
            db.CheckingAccounts.Add(checkingAccount);
            db.SaveChanges();
        }
    }
}