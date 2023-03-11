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


        //SQL CONNECTIONS AND TRANSACTIONS
        Transfer SendTransactionToOtherUser(Account accountFrom, Account accountTo, decimal amountToTransfer);
        Transfer RequestTransferFromOtherUser(Account requestingAccountId, Account SendingAccountId, decimal amountToTransfer);
        Transfer ApprovePendingRequest(Account requestingAccount, Account requestedAccount, int originalTransferId);
        Transfer DeclinePendingRequest(Account requestingAccount, Account requestedAccount, int originalTransferId);
        // SQL COMMANDS
        int CreateTransaction(SqlConnection conn, SqlTransaction transaction, int transferType, int transferStatus, int accountFromId, int accountToId, decimal amountToTransfer);
        void UpdateBalance(SqlConnection conn, SqlTransaction transaction, int userId, decimal newBalance, int accountId);
        int UpdateTransferStatus(SqlConnection conn, SqlTransaction transaction, int transferId, int newStatusId);
    }
}
