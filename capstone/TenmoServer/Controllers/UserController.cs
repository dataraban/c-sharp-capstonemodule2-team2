using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountDao accountDao;
        private readonly IUserDao userDao;

        public UserController(IAccountDao accountDao, IUserDao userDao)
        {
            this.accountDao = accountDao;
            this.userDao = userDao;
        }

        [HttpGet("{user_id}/balance")]
        public ActionResult<decimal> GetBalance(int user_id)
        {
            int accountId = accountDao.GetAccountByUser(user_id).AccountId;
            return accountDao.AccountBalance(accountId);
        }

        [HttpGet("{user_id}/account")]
        public ActionResult<Account> GetAccountByUserId(int user_id)
        {
            return accountDao.GetAccountByUser(user_id);
        }

        [HttpGet("{user_id}")]
        public ActionResult<decimal> GetUserInfo(int user_id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<IList<User>> GetAllUsers()
        {
            IList<User> users = userDao.GetUsers();

            if(users.Count == 0)
            {
                return NoContent(); 
            }
            return Ok(users);
        }

    }
}
