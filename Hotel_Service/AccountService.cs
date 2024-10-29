using Hotel_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepo accountRepo;
        public AccountService()
        {
            accountRepo = new AccountRepo();
        }
        public List<string> GetAccount() => accountRepo.GetAccount();
    }
}
