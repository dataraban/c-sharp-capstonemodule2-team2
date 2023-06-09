﻿using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        public int request = 1;
        public int send = 2;

        public int pending = 1;
        public int approved = 2;
        public int rejected = 3;


        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        /*
         *   ----------------------------READING------------------------------
         */

        public IList<Transfer> GetAllTransfersByUser(int userId)  // return a list of all the users transactions
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

        public Transfer GetTransferByTransferId(int transferId)  //Get a single transaction by ID
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
            catch (Exception)
            {
                Console.WriteLine("Error getting transaction by transferId");
            }
            return transfer;
        }

        public int? GetTransferStatus(int transactionId)  //get the status of a transaction 
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

        public Transfer CreateTransferFromReader(SqlDataReader sdr)
        {
            Transfer transfer = new Transfer
            {
                TransferId = Convert.ToInt32(sdr["transfer_id"]),
                TransferTypeId = Convert.ToInt32(sdr["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(sdr["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(sdr["account_from"]),
                AccountTo = Convert.ToInt32(sdr["account_to"]),
                Amount = Convert.ToDecimal(sdr["amount"])
            };
            return transfer;
        }



        /*
         *   ----------------------------WRITING------------------------------
         */

        //SQL CONNECTIONS AND TRANSACTIONS
        public Transfer SendTransactionToOtherUser(Account accountFrom, Account accountTo, decimal amountToTransfer)
        {
            int newTransferId;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    newTransferId = CreateTransaction(conn, transaction, send, approved, accountFrom.AccountId, accountTo.AccountId, amountToTransfer);
                    UpdateBalance(conn, transaction, accountFrom.UserId, accountFrom.Balance - amountToTransfer, accountFrom.AccountId);
                    UpdateBalance(conn, transaction, accountTo.UserId, accountTo.Balance + amountToTransfer, accountTo.AccountId);

                    transaction.Commit();
                    return GetTransferByTransferId(newTransferId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error creating new transfer");
                return null;
            }

        }

        public Transfer RequestTransferFromOtherUser(Account requestingAccount, Account requestedAccount, decimal amountToTransfer)
        {
            int newTransferId;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    newTransferId = CreateTransaction(conn, transaction, request, pending, requestingAccount.AccountId, requestedAccount.AccountId, amountToTransfer);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error creating Request Transfer");
                return null;
            }
            return GetTransferByTransferId(newTransferId);
        }

        public Transfer ApprovePendingRequest(Account requestingAccount, Account requestedAccount, int originalTransferId)
        {
            int updatedTransferId;
            Transfer originalTransfer = GetTransferByTransferId(originalTransferId);
            if (originalTransfer == null)
            {
                return null;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    updatedTransferId = UpdateTransferStatus(conn, transaction, originalTransfer.TransferId, approved);
                    UpdateBalance(conn, transaction, requestedAccount.UserId, requestedAccount.Balance - originalTransfer.Amount, requestedAccount.AccountId);
                    UpdateBalance(conn, transaction, requestingAccount.UserId, requestingAccount.Balance + originalTransfer.Amount, requestingAccount.AccountId);

                    transaction.Commit();
                    return GetTransferByTransferId(updatedTransferId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error creating new transfer");
                return null;
            }
        }

        public Transfer DeclinePendingRequest(Account requestingAccount, Account requestedAccount, int originalTransferId)
        {
            int updatedTransferId;
            Transfer originalTransfer = GetTransferByTransferId(originalTransferId);
            if (originalTransfer == null)
            {
                return null;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    updatedTransferId = UpdateTransferStatus(conn, transaction, originalTransfer.TransferId, rejected);

                    transaction.Commit();
                    return GetTransferByTransferId(updatedTransferId);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error creating new transfer");
                return null;
            }
        }
       
        // SQL COMMANDS
        public int CreateTransaction(SqlConnection conn, SqlTransaction transaction, int transferType, int transferStatus, int accountFromId, int accountToId, decimal amountToTransfer)
        {
            SqlCommand cmdCreateTransfer = new SqlCommand("" +
                        "INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                        "OUTPUT INSERTED.transfer_id " +
                        "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);"
                        , conn, transaction);
            cmdCreateTransfer.Parameters.AddWithValue("@transfer_type_id", transferType);
            cmdCreateTransfer.Parameters.AddWithValue("@transfer_status_id", transferStatus);
            cmdCreateTransfer.Parameters.AddWithValue("@account_from", accountFromId);
            cmdCreateTransfer.Parameters.AddWithValue("@account_to", accountToId);
            cmdCreateTransfer.Parameters.AddWithValue("@amount", amountToTransfer);
            return Convert.ToInt32(cmdCreateTransfer.ExecuteScalar());
        }

        public void UpdateBalance(SqlConnection conn, SqlTransaction transaction, int userId, decimal newBalance, int accountId)
        {
            SqlCommand cmdUpdateSenderBalance = new SqlCommand("" +
                        "UPDATE account " +
                        "SET user_id = @userId, balance = @balance " +
                        "WHERE account_id = @account_id;"
                        , conn, transaction);
            cmdUpdateSenderBalance.Parameters.AddWithValue("@userId", userId);
            cmdUpdateSenderBalance.Parameters.AddWithValue("@balance", newBalance);
            cmdUpdateSenderBalance.Parameters.AddWithValue("@account_id", accountId);
            cmdUpdateSenderBalance.ExecuteNonQuery();
        }
        
        public int UpdateTransferStatus(SqlConnection conn, SqlTransaction transaction, int transferId, int newStatusId)
        {
            
            Transfer oldTransfer = GetTransferByTransferId(transferId);
            SqlCommand cmd = new SqlCommand("" +
                        "Update transfer " +
                        "SET transfer_type_id = @transfer_type_id, transfer_status_id = @transfer_status_id, account_from = @account_from, account_to = @account_to, amount = @amount) " +
                        "WHERE transfer_id = @transfer_id"
                        , conn, transaction);
            cmd.Parameters.AddWithValue("@transfer_type_id", oldTransfer.TransferTypeId);
            cmd.Parameters.AddWithValue("@transfer_status_id", newStatusId);
            cmd.Parameters.AddWithValue("@account_from", oldTransfer.AccountFrom);
            cmd.Parameters.AddWithValue("@account_to", oldTransfer.AccountTo);
            cmd.Parameters.AddWithValue("@amount", oldTransfer.Amount);

            cmd.ExecuteNonQuery();

            return transferId;
        }

    }
}
