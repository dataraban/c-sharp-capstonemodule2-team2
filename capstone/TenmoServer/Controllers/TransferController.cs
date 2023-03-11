using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDao transferDao;
        private readonly IAccountDao accountDao;
        public int pending = 1;
        public int approved = 2;
        public int rejected = 3;

        public TransferController(ITransferDao transferDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.accountDao = accountDao;
        }

        /*
         *   ----------------------------READING------------------------------
         */

        [HttpGet("{userId}/pasttransfers")]
        public ActionResult<IList<Transfer>> GetPastTransfers(int userId)
        {
            if (!VerifyLoggedInUserId(userId))
            {
                return Forbid();
            }
            else
            {
                IList<Transfer> transfers = transferDao.GetAllTransfersByUser(userId);
                if (transfers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(transfers);
            }
        }

        [HttpGet("status/{transferId}")]
        public ActionResult<string> TransactionStatus(int transferId)
        {
            if (!TransferExists(transferId))
            {
                return NoContent();
            }
            else if (!VerifyLoggedInUserbyTransferId(transferId))
            {
                return Forbid();
            }
            else
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
        }

        [HttpGet("{transferId}")]
        public ActionResult<Transfer> GetByTransferId(int transferId)
        {
            if (!TransferExists(transferId))
            {
                return NoContent();
            }
            else if (!VerifyLoggedInUserbyTransferId(transferId))
            {
                return Forbid();
            }
            else
            {
                Transfer result = transferDao.GetTransferByTransferId(transferId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
        }

        /*
         *   ----------------------------WRITING------------------------------
         */

        [HttpPost("send")]
        public ActionResult<Transfer> SendToOtherUser(SendTransfer sendTransfer)
        {
            if (!VerifyLoggedInUserId(sendTransfer.UserIdFrom))
            {
                return Forbid();
            }
            else
            {
                Account accountFrom = accountDao.GetAccountByUser(sendTransfer.UserIdFrom);
                Account accountTo = accountDao.GetAccountByUser(sendTransfer.UserIdTo);
                Transfer newTransfer = transferDao.SendTransactionToOtherUser(accountFrom, accountTo, sendTransfer.AmountToSend);
                return newTransfer;
            }
        }

        [HttpPost("request")]
        public ActionResult<Transfer> RequestFromOtherUser(RequestTransfer requestTransfer)
        {
            if (!VerifyLoggedInUserId(requestTransfer.RequestingUserId))
            {
                return Forbid();
            }
            else
            {
                Account accountFrom = accountDao.GetAccountByUser(requestTransfer.RequestingUserId);
                Account accountTo = accountDao.GetAccountByUser(requestTransfer.RequestedUserId);
                Transfer newTransfer = transferDao.RequestTransferFromOtherUser(accountFrom, accountTo, requestTransfer.AmountToRequest);
                return newTransfer;
            }
        }

        [HttpPut("{transferId}")]
        public ActionResult<Transfer> UpdatePendingToApprovedOrRejected(UpdateTransfer updateTransfer)
        {
            Transfer inputTransfer = transferDao.GetTransferByTransferId(updateTransfer.TransferId);
            Account accountFrom = accountDao.GetAccountByAccountId(inputTransfer.AccountFrom);
            Account accountTo = accountDao.GetAccountByAccountId(inputTransfer.AccountTo);
            Transfer updatedTransfer;

            if (inputTransfer == null)
            {
                return NotFound(inputTransfer);
            }
            else if (!VerifyLoggedInUserbyTransferId(updateTransfer.TransferId))
            {
                return Forbid();
            }
            else if (updateTransfer.NewStatusCodeId == approved)
            {
                updatedTransfer = transferDao.ApprovePendingRequest(accountFrom, accountTo, inputTransfer.TransferId);
                return updatedTransfer;
            }
            else if (updateTransfer.NewStatusCodeId == rejected)
            {
                updatedTransfer = transferDao.DeclinePendingRequest(accountFrom, accountTo, inputTransfer.TransferId);
                return updatedTransfer;
            }
            else
            {
                return NotFound();
            }
        }


        public bool TransferExists(int transferId)
        {
            if (transferDao.GetTransferByTransferId(transferId) == null)
            {
                return false;
            }
            return true;
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

        public bool VerifyLoggedInUserbyTransferId(int transferId)
        {
            int loggedInUser = Convert.ToInt32(User.FindFirst("sub")?.Value);
            Transfer transfer = transferDao.GetTransferByTransferId(transferId);
            int userId1 = accountDao.GetUserIdByAccountId(transfer.AccountFrom);
            int userId2 = accountDao.GetUserIdByAccountId(transfer.AccountTo);

            if (userId1 != loggedInUser && userId2 != loggedInUser)
            {
                return false;
            }
            return true;
        }
    }
}
