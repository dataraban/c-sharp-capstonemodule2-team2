using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDao accountDao;

        public AccountController(IAccountDao accountDao)
        {
            this.accountDao = accountDao;
        }

        [HttpGet]
        public ActionResult<Account> GetBalance(int user_id)
        {
            return accountDao.GetAccountByUser(user_id);
        }

    }
}
