using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }



        public Transaction GetTransaction(int transactionId)  //Get a single transaction by ID
        {

        }


        public int GetTransactionStatus(int transactionId)  //get the status of a transaction 
        {

        }

        public Transaction SendTransactionToOtherUser(int userIdFrom, int userIdTo, decimal amountToTransfer) // use case 4 - this method probably needs to call smaller sub methods IMO
        {

        }

        public IList<Transaction> GetAllTransactionsByUser(int userId)  // return a list of all the users transactions
        {

        }




    }
}
