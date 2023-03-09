using RestSharp;
using System;
using System.Collections.Generic;
using TenmoClient.Models;

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

        //TO DO!
        internal List<Transfer> GetPastTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();
            //RestRequest request = new RestRequest($"user/{UserId}/tr");
            //IRestResponse<decimal> response = client.Get<decimal>(request);

            //CheckForError(response);
            return transfers;
        }
    }
}
