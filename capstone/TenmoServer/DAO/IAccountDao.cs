using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccount(int accountId);
        IList<Account> GetAccountsByUser(int userId);
        decimal AccountBalance(int balance);





    }
}
