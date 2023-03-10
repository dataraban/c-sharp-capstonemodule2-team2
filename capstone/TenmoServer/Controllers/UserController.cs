using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
            if (!VerifyLoggedInUserId(user_id))
            {
                return Forbid();
            }
            int accountId = accountDao.GetAccountByUser(user_id).AccountId;
            return accountDao.AccountBalance(accountId);
        }

        [HttpGet("{user_id}/account")]
        public ActionResult<Account> GetAccountByUserId(int user_id)
        {
            if(!VerifyLoggedInUserId(user_id))
            {
                return Forbid();
            }
            else
            {
                Account returnAccount = accountDao.GetAccountByUser(user_id);
                if(returnAccount == null)
                {
                    return NotFound();
                }
                return returnAccount;
            }
        }

        [HttpGet("{user_id}")]
        public ActionResult<decimal> GetUserInfo(int user_id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{account_id}/username")]
        public ActionResult<string> GetUsernameFromAccount(int account_id)
        {
            return accountDao.GetUsernameByAccountId(account_id);
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

        public bool VerifyLoggedInUserId(int userId)
        {
            int loggedInUser = Convert.ToInt32(User.FindFirst("sub")?.Value);
            
            if (userId != loggedInUser)
            {
                return false;
            }
            return true;
        }

    }
}
