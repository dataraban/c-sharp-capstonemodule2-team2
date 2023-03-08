using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDao transferDao;
        private readonly IAccountDao accountDao;

        public TransferController(ITransferDao transferDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.accountDao = accountDao;
        }

        [HttpGet("{user_id}/pasttransfers")]
        public ActionResult<IList<Transfer>> GetPastTransfers(int user_id)
        {
            int accountId = accountDao.GetAccountByUser(user_id).AccountId;
            IList<Transfer> transfers = transferDao.GetAllTransactionsByUser(accountId);
            if(transfers.Count == 0)
            {
                return NoContent();
            }
            return Ok(transfers);
        }

        //[HttpGet]


        //[HttpGet("")]

    }
}
