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
        Transfer SendTransactionToOtherUser(int accountIdFrom, int accountIdTo, decimal amountToTransfer);
        Transfer RequestTransferFromOtherUser(int requestingAccountId, int SendingAccountId, decimal amountToTransfer);
        Transfer CreateNewTransfer(int accountIdFrom, int accountIdTo, decimal amountToTransfer, int transferType, int transferStatus);
        Transfer UpdateTransferStatus(int transferId, int newStatusId);
    }
}
