using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;
using System.Transactions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }



        public Transfer GetTransaction(int transferId)  //Get a single transaction by ID
        {
            Transfer transfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfer WHERE transfer_id = @transferId", conn);
                    cmd.Parameters.AddWithValue("@transferId", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        transfer = CreateTransferFromReader(reader);
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Error getting transaction by transferId");
            }
            return transfer;
        }


        public IList<Transfer> GetAllTransactionsByUser(int userId)  // return a list of all the users transactions
        {
            IList<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM transfer " +
                        "JOIN account on account.account_id = transfer.account_from OR account.account_id = transfer.account_to " +
                        "WHERE user_id = @userId"
                        , conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transfer transfer = CreateTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Error getting all transactions by userId");
            }
            return transfers;
        }


        public int? GetTransactionStatus(int transactionId)  //get the status of a transaction 
        {
            int? transactionStatus = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT transfer_type_id FROM transfer WHERE transfer_id = @transferId", conn);
                    cmd.Parameters.AddWithValue("@transferId", transactionId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        transactionStatus = Convert.ToInt32(reader["transfer_type_id"]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error getting transaction status by transferId");
            }
            return transactionStatus;
        }

        public Transfer SendTransactionToOtherUser(int userIdFrom, int userIdTo, decimal amountToTransfer) // use case 4 - this method probably needs to call smaller sub methods IMO
        {
            //create transaction
            
            //update one

            //update other

            throw new NotImplementedException();
        }

        public Transfer CreateNewTransfer(int userIdFrom, int userIdTo, decimal amountToTransfer)
        {
            throw new NotImplementedException();
        }


        public Transfer CreateTransferFromReader(SqlDataReader sdr)
        {
            Transfer transfer = new Transfer
            {
                TransferId = Convert.ToInt32(sdr["transfer_id"]),
                TransferTypeID = Convert.ToInt32(sdr["transfer_type_id"]),
                TransferStatusID = Convert.ToInt32(sdr["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(sdr["account_from"]),
                AccountTo = Convert.ToInt32(sdr["account_to"]),
                Amount = Convert.ToDecimal(sdr["amount"])
            };
            return transfer;
        }



    }
}
