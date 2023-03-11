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

        public void ViewPastTransfers(List<PastTransfer> transfers)
        {

            Console.WriteLine($"-----------------------------------------------------------------------");
            Console.WriteLine("{0,-50}", "Transfers");
            Console.WriteLine("{0,-6} {1,-21} {2,10} {3,-10}", "ID", "From/To", "Amount", "Status");
            Console.WriteLine($"-----------------------------------------------------------------------");

            if (transfers != null)
            {
                foreach (PastTransfer t in transfers)
                {
                    Console.WriteLine("{0,-6} {1,-5} {2,-15} {3,10:C2} {4,-8} {5,-10}", t.TransferId, t.TransferTypeAction + ":", t.UsernameToFrom, t.Amount, "Status: ", t.TransferStatus);
                    //Console.WriteLine(          "{0,5} {1,-5} {2,20} {3,8}", "TestID", "From:", "TestUsername", "$123.45");
                }
            }
            else
            {
                Console.WriteLine("Unable to retrieve past transfers.");
            }
        }


        public void DisplayUsers(List<User> users)
        {
            Console.WriteLine($"------------------------Users--------------------------");
            //Console.WriteLine("{0,5} {1,25}", "", "Users");
            //Console.WriteLine($"-------------------------------------------------------");
            Console.WriteLine("{0,5} {1,5} {2,-35}", "ID", " | ", "Username");
            Console.WriteLine($"-------------------------------------------------------");

            foreach (User u in users)
            {
                Console.WriteLine("{0,5} {1,5} {2,-35}", u.UserId, " | ", u.Username);
            }
        }

        internal void ViewTransferDetails(PastTransfer transfer)
        {
            Console.WriteLine($"-----------------------------------------------------------------------");
            Console.WriteLine("{0,-50}", "Transfer Details");
            Console.WriteLine($"-----------------------------------------------------------------------");
            if(transfer != null )
            {
                Console.WriteLine("{0,-10} {1,-25}", "Id: ", transfer.TransferId);
                Console.WriteLine("{0,-10} {1,-25}", "From: ", transfer.UsernameFrom);
                Console.WriteLine("{0,-10} {1,-25}", "To: ", transfer.UsernameTo);
                Console.WriteLine("{0,-10} {1,-25}", "Type: ", transfer.TransferType);
                Console.WriteLine("{0,-10} {1,-25}", "Status: ", transfer.TransferStatus);
                Console.WriteLine("{0,-10} {1,-25:C2}", "Amount: ", transfer.Amount);
            }
            else
            {
                Console.WriteLine("Unable to retrieve transfer details.");
            }
        }
    }
}
