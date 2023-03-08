using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountDao accountDao;

        public UserController(IAccountDao accountDao)
        {
            this.accountDao = accountDao;
        }

        [HttpGet("{user_id}/balance")]
        public ActionResult<decimal> GetBalance(int user_id)
        {
            int accountId = accountDao.GetAccountByUser(user_id).AccountId;
            return accountDao.AccountBalance(accountId);
        }

        [HttpGet("{user_id}")]
        public ActionResult<decimal> GetUserInfo(int user_id)
        {
            throw new NotImplementedException();
        }

    }
}
