using System.Transactions;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        public Transaction GetTransaction(string transactionId);

    }
}
