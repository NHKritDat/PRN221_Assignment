using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Repositories
{
    public interface IAccountRepo
    {
        public List<string> GetAccount();
    }
}
