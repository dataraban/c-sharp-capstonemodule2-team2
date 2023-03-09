using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        // Add application-specific UI methods here...

        public void ViewBalance(decimal balance)
        {
            Console.WriteLine($"Your current account balance is: {balance:C2}");
        }

        public void ViewPastTransfers(List<Transfer> transfers)
        {

            Console.WriteLine($"-------------------------------------------------------");
            Console.WriteLine("{0,-50}", "Transfers");
            Console.WriteLine("{0,-10} {1,-5} {2,25} {3,5}", "ID", "From/To","", "Amount");
            Console.WriteLine($"-------------------------------------------------------");
            //Console.WriteLine("{0,-10} {1,-5} {2,20} {3,8} {4,1}", "TestID", "From:", "TestUsername", "$", "123.45");
            foreach (Transfer t in transfers)
            {
                Console.WriteLine("{0,-10} {1,-5} {2,20} {3,8} {4,1}", t.TransferId, t.TransferTypeName + ":", t.UsernameToFrom, "$", t.Amount);
                //Console.WriteLine(          "{0,5} {1,-5} {2,20} {3,8}", "TestID", "From:", "TestUsername", "$123.45");
            }
        }
    }
}
