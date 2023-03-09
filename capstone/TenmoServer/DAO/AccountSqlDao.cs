using System;
using System.Collections;
using System.Data.SqlClient;
using System.Transactions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string connString)
        { connectionString = connString; }




        public Account GetAccountByAccountId(int accountId)
        {
            Account account = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE account_id = @account_id;", conn);
                cmd.Parameters.AddWithValue("@account_id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    account = CreateAccountFromReader(reader);
                }
                return account;

            }
        }
        public Account GetAccountByUser(int userId)
        {
            Account account = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @user_id;", conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    account = CreateAccountFromReader(reader);
                }
                return account;

            }
        }
        
        public Account CreateAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account();
            account.AccountId = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);

            return account;



        }

        public decimal AccountBalance(int accountId)
        {
            Account account = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE account_id = @account_id;", conn);
                cmd.Parameters.AddWithValue("@account_id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    account = CreateAccountFromReader(reader);
                }
                return account.Balance;

            }
        }
        public int GetUserIdByAccountId(int accountId)
        {
            int userId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("" +
                        "SELECT user_id FROM account WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error getting User ID by Account ID");
            }
            return userId;
        }
        public string GetUsernameByAccountId(int accountId) 
        {
            string username = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("" +
                        "SELECT username FROM account " +
                        "JOIN tenmo_user ON tenmo_user.user_id = account.user_id " +
                        "WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        username = Convert.ToString(reader["username"]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error getting Username by Account ID");
            }
            return username;
        }



    }
} 

