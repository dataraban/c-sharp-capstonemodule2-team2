using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccount(int accountId);
        Account GetAccountByUser(int userId);
        decimal AccountBalance(int accountId);





    }
}
