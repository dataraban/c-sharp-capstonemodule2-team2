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

        public List<PastTransfer> GetPastTransfersWithUsernames(int transferTypeIdFilter = 0, int transferStatusIdFilter = 0)
        {
            List<Transfer> allPastTransfers = GetPastTransfers();
            if(allPastTransfers == null)
            {
                return null;
            }
            List<PastTransfer> pastTransfersWithUsernames = new List<PastTransfer>();
            foreach (Transfer transfer in allPastTransfers)
            {
                if(transferTypeIdFilter != 0 && transferTypeIdFilter != transfer.TransferTypeId && transferStatusIdFilter != transfer.TransferStatusId)
                {
                    continue;
                }
                PastTransfer transferWithUsername = new PastTransfer(transfer);

                transferWithUsername.UsernameTo = GetUsernameFromAccount(transfer.AccountTo);
                transferWithUsername.UsernameFrom = GetUsernameFromAccount(transfer.AccountFrom);
                transferWithUsername.SetToFrom(Username);
                pastTransfersWithUsernames.Add(transferWithUsername);
            }
            return pastTransfersWithUsernames;
        }

        public PastTransfer GetPastTransferWithUsernames(int transferId)
        {
            Transfer transfer = GetTransferDetails(transferId);
            if (transfer == null)
            {
                return null;
            }
            PastTransfer transferWithUsernames = new PastTransfer(transfer);

            transferWithUsernames.UsernameTo = GetUsernameFromAccount(transfer.AccountTo);
            transferWithUsernames.UsernameFrom = GetUsernameFromAccount(transfer.AccountFrom);
            
            return transferWithUsernames;
        }

        private string GetUsernameFromAccount(int account)
        {
            RestRequest request = new RestRequest($"user/{account}/username");
            IRestResponse<string> response = client.Get<string>(request);
            CheckForError(response);
            return response.Data;
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

        public Transfer RequestTransfer(decimal amountToRequest, int userIdSelection)
        {
            RequestTransfer newTransfer = new RequestTransfer(UserId, userIdSelection, amountToRequest);
            RestRequest request = new RestRequest($"transfer/request");
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

        public Transfer GetTransferDetails(int transferId)
        {
            RestRequest request = new RestRequest($"transfer/{transferId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            CheckForError(response);
            return response.Data;
        }

        public Transfer RequestTransfer(decimal amountToRequest, int userIdSelection)
        {
            ReceiveTransfer newTransfer = new ReceiveTransfer(UserId, userIdSelection, amountToRequest);
            RestRequest request = new RestRequest($"transfer/request");
            request.AddJsonBody(newTransfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }
    }
}
