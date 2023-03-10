using RestSharp;
using System;
using System.Collections.Generic;
using TenmoClient.Models;
using static TenmoClient.Models.Transfer;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public decimal GetBalance()
        {
            RestRequest request = new RestRequest($"user/{UserId}/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetPastTransfers()
        {
            RestRequest request = new RestRequest($"transfer/{UserId}/pasttransfers");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<PastTransfer> GetPastTransfersWithUsernames()
        {
            List<Transfer> allPastTransfers = GetPastTransfers();
            List<PastTransfer> pastTransfersWithUsernames = new List<PastTransfer>();
            foreach (Transfer transfer in allPastTransfers)
            {
                PastTransfer transferWithUsername = new PastTransfer 
                {
                    TransferId = transfer.TransferId,
                    TransferTypeId = transfer.TransferTypeId,
                    TransferStatusId = transfer.TransferStatusId,
                    AccountFrom = transfer.AccountFrom,
                    AccountTo = transfer.AccountTo,
                    Amount = transfer.Amount
                };

                RestRequest request = new RestRequest($"user/{transfer.AccountFrom}/username");
                IRestResponse<string> response = client.Get<string>(request);
                CheckForError(response);
                transferWithUsername.UsernameFrom = response.Data;

                request = new RestRequest($"user/{transfer.AccountTo}/username");
                response = client.Get<string>(request);
                CheckForError(response);
                transferWithUsername.UsernameTo = response.Data;
                pastTransfersWithUsernames.Add(transferWithUsername);
            }
            return pastTransfersWithUsernames;
        }

        public List<User> GetUsersForTransfers()
        {
            List<User> users = new List<User>();
            RestRequest request = new RestRequest($"user");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            CheckForError(response);
            return RemoveCurrentUserFromList(response.Data);
        }

        public Transfer SendTransfer(decimal amountToSend, int userIdSelection)
        {
            SendTransfer newTransfer = new SendTransfer(UserId, userIdSelection, amountToSend);
            RestRequest request = new RestRequest($"transfer/send");
            request.AddJsonBody(newTransfer);
            
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }



        private List<User> RemoveCurrentUserFromList(List<User> users)
        {
            users.RemoveAll(x => x.UserId == this.UserId);
            return users;
        }
    }
}
