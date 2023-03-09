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
    public class TransferController : ControllerBase
    {
        private readonly ITransferDao transferDao;
        private readonly IAccountDao accountDao;

        public TransferController(ITransferDao transferDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.accountDao = accountDao;
        }

        /*
         *   ----------------------------READING------------------------------
         */

        [HttpGet("{user_id}/pasttransfers")]
        public ActionResult<IList<Transfer>> GetPastTransfers(int user_id)
        {
            //int accountId = accountDao.GetAccountByUser(user_id).AccountId;
            IList<Transfer> transfers = transferDao.GetAllTransfersByUser(user_id);
            if(transfers.Count == 0)
            {
                return NoContent();
            }
            return Ok(transfers);
        }

        [HttpGet("{transferId}")]
        public ActionResult<string> TransactionStatus(int transferId)
        {
            int? status = transferDao.GetTransferStatus(transferId);
            switch (status)
            {
                case 1:
                    return Ok("Pending");
                case 2:
                    return Ok("Approved");
                case 3:
                    return Ok("Rejected");
            }
            return NotFound("Could not retrieve transfer status");
        }

        [HttpGet("status/{transferId}")]
        public ActionResult<Transfer> GetByTransferId(int transferId)
        {
            Transfer result = transferDao.GetTransferByTransferId(transferId);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /*
         *   ----------------------------WRITING------------------------------
         */

        [HttpPost("send")]
        //[HttpPost()]
        public ActionResult<Transfer> SendToOtherUser(SendTransfer sendTransfer)
        {
            Account accountFrom = accountDao.GetAccountByUsername(sendTransfer.UsernameFrom);
            Account accountTo = accountDao.GetAccountByUsername(sendTransfer.UsernameTo);
            Transfer newTransfer = transferDao.SendTransactionToOtherUser(accountFrom, accountTo, sendTransfer.AmountToSend);
            return newTransfer;
        }

        [HttpPost("request")]
        public ActionResult<Transfer> RequestFromOtherUser(string requestingUsername, string requestedUsername, decimal amountToTransfer)
        {
            Account accountFrom = accountDao.GetAccountByUsername(requestingUsername);
            Account accountTo = accountDao.GetAccountByUsername(requestedUsername);
            Transfer newTransfer = transferDao.RequestTransferFromOtherUser(accountFrom, accountTo, amountToTransfer);
            return newTransfer;
        }

        [HttpPut("{transferId}")]
        public ActionResult<Transfer> UpdatePendingApprovedOrRejected(int transferId, int newStatusCodeId)
        {
            Transfer updatedTransfer = transferDao.UpdateTransferStatus(transferId, newStatusCodeId);
            if (updatedTransfer == null)
            {
                return NotFound(updatedTransfer);
            }
           return Ok(updatedTransfer);
        }
    }
}
