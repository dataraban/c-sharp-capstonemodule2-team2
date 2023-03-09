using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        //READING
        IList<Transfer> GetAllTransfersByUser(int userId);
        Transfer GetTransferByTransferId(int transferId);
        int? GetTransferStatus(int transactionId);
        Transfer CreateTransferFromReader(SqlDataReader sdr);
        
        
        // WRITING
        Transfer SendTransactionToOtherUser(Account accountFrom, Account accountTo, decimal amountToTransfer);
        Transfer RequestTransferFromOtherUser(Account requestingAccountId, Account SendingAccountId, decimal amountToTransfer);
        Transfer UpdateTransferStatus(int transferId, int newStatusId);
    }
}
