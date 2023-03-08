using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransaction(int transferId);
        IList<Transfer> GetAllTransactionsByUser(int userId);
        int? GetTransactionStatus(int transactionId);
        Transfer SendTransactionToOtherUser(int userIdFrom, int userIdTo, decimal amountToTransfer);
        Transfer CreateTransferFromReader(SqlDataReader sdr);
    }
}
