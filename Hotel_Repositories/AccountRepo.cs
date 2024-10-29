using HotelUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Repositories
{
    public class AccountRepo : IAccountRepo
    {
        public List<string> GetAccount() => FileUtil<string>.GetAccount();
    }
}
