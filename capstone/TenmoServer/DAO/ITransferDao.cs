using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        //READING
        Transfer GetTransaction(int transferId);
        IList<Transfer> GetAllTransactionsByUser(int userId);
        int? GetTransactionStatus(int transactionId);
        Transfer CreateTransferFromReader(SqlDataReader sdr);
        
        
        // WRITING
        Transfer SendTransactionToOtherUser(int accountIdFrom, int accountIdTo, decimal amountToTransfer);
        Transfer RequestTransferFromOtherUser(int requestingAccountId, int SendingAccountId, decimal amountToTransfer);
        Transfer CreateNewTransfer(int accountIdFrom, int accountIdTo, decimal amountToTransfer, int transferType, int transferStatus);
        void UpdateTransferStatus(int transferId, int newStatusId);
    }
}
