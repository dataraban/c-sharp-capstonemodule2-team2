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

        public List<PastTransfer> GetPastTransfers()
        {
            //List<Transfer> transfers = new List<Transfer>();
            RestRequest request = new RestRequest($"transfer/{UserId}/pasttransfers");
            IRestResponse<List<PastTransfer>> response = client.Get<List<PastTransfer>>(request);

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

        private List<User> RemoveCurrentUserFromList(List<User> users)
        {
            users.RemoveAll(x => x.UserId == this.UserId);
            return users;
        }
    }
}
