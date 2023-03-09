﻿using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                //Console.WriteLine("This should display the balance.");
                //console.Pause();
                console.ViewBalance(tenmoApiService.GetBalance());
                console.Pause();


            }

            if (menuSelection == 2)
            {
                // View your past transfers
                console.ViewPastTransfers(tenmoApiService.GetPastTransfers());
                //console.ViewPastTransfers()
                console.Pause();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                // Send TE bucks

                //get and validate user for transfer
                List<User> usersForTransfers = tenmoApiService.GetUsersForTransfers();
                console.DisplayUsers(usersForTransfers);
                int userIdSelection = console.PromptForInteger("Id of the user you are sending to", 0, int.MaxValue);
                bool isValidUserIdSelection = VerifyValidUserIdSelection(userIdSelection, usersForTransfers);
                while (!isValidUserIdSelection)
                {
                    console.PrintError("Not a valid user Id. Please try again or press 0 to cancel.");
                    userIdSelection = console.PromptForInteger("Id of the user you are sending to", 0, int.MaxValue);
                    if (userIdSelection == 0)
                    {
                        RunAuthenticated();
                    }
                    isValidUserIdSelection = VerifyValidUserIdSelection(userIdSelection, usersForTransfers);
                }

                //get and validate amount to send             
                decimal amountToSend = console.PromptForDecimal("Enter amount to send");
                bool isValidAmountToSend = ValidateAmountToSend(amountToSend);
                while (!isValidAmountToSend)
                {                    
                    amountToSend = console.PromptForDecimal("Enter amount to send");
                    isValidAmountToSend = ValidateAmountToSend(amountToSend);
                }

                //complete transfer
                console.Pause($"Sending {amountToSend:C2} to user {userIdSelection}...");

            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private bool ValidateAmountToSend(decimal amountToSend)
        {
            bool isValidAmountToSend = true;
            if(amountToSend <= 0)
            {
                console.PrintError("Amount must be more than $0");
                return false;
            }

            decimal balance = tenmoApiService.GetBalance();

            if (amountToSend > balance)
            {
                console.PrintError($"Amount must be less than or equal to your balance of {balance:C2}.");
                return false;
            }
            return isValidAmountToSend;
        }

        private bool VerifyValidUserIdSelection(int userIdSelection, List<User> usersForTransfers)
        {
            bool isValidUserIdSelection = false;
            foreach (User u in usersForTransfers)
            {
                if (u.UserId == userIdSelection)
                {
                    isValidUserIdSelection = true;
                }
            }
            return isValidUserIdSelection;
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
    }
}
