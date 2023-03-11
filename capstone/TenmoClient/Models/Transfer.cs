
using System.ComponentModel.DataAnnotations;
using System;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public int TransferStatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public string TransferStatus
        {
            get
            {
                switch (TransferStatusId)
                {
                    case 1:
                        return "Pending";
                    case 2:
                        return "Approved";
                    case 3:
                        return "Rejected";
                    default:
                        return "Unknown";
                }
            }
        }
    }

    public class PastTransfer : Transfer
    {
       
        public string UsernameTo { get; set; }
        public string UsernameFrom { get; set; }
        //public string UsernameToFrom
        //{
        //    get
        //    {
        //        if (TransferTypeId == 1) return UsernameFrom;
        //        else if (TransferTypeId == 2) return UsernameTo;
        //        else return "Unknown";
        //    }
        //}

        public string UsernameToFrom
        {
            get; set;
        }
        public string TransferTypeAction
        {
            get;
            //{
            //    if (TransferTypeId == 1) return "From";
            //    else if (TransferTypeId == 2) return "To";
            //    else return "Unknown";
            //}
            set;
        }


        public string TransferType
        {
            get
            {
                if (TransferTypeId == 1) return "Request";
                else if (TransferTypeId == 2) return "Send";
                else return "Unknown";
            }
        }

        public PastTransfer(Transfer transfer)
        {
            TransferId = transfer.TransferId;
            TransferTypeId = transfer.TransferTypeId;
            TransferStatusId = transfer.TransferStatusId;
            AccountFrom = transfer.AccountFrom;
            AccountTo = transfer.AccountTo;
            Amount = transfer.Amount;
        }

        public void SetToFrom(string currentUser)
        {
            if(TransferTypeId == 1)
            {
                if (currentUser == UsernameFrom)
                {
                    TransferTypeAction = "From";
                    UsernameToFrom = UsernameTo;
                }
                else if (currentUser == UsernameTo)
                {
                    TransferTypeAction = "To";
                    UsernameToFrom = UsernameFrom;
                }
            }
            else if(TransferTypeId == 2)
            {
                if(currentUser == UsernameFrom)
                {
                    TransferTypeAction = "To";
                    UsernameToFrom = UsernameTo;
                }
                else if(currentUser == UsernameTo)
                {
                    TransferTypeAction = "From";
                    UsernameToFrom = UsernameFrom;
                }
            }
        }
    }



    public class SendTransfer
    {
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public decimal AmountToSend { get; set; }

        public SendTransfer(int userIdFrom, int userIdTo, decimal amountToSend)
        {
            UserIdFrom = userIdFrom;
            UserIdTo = userIdTo;
            AmountToSend = amountToSend;
        }
    }

    public class ReceiveTransfer
    {
        
        public int RequestingUserId { get; set; }
        
        public int RequestedUserId { get; set; }

        public decimal AmountToRequest { get; set; }

        public ReceiveTransfer(int requestingUserId, int requestedUserId, decimal amountToRequest)
        {
            RequestingUserId = requestingUserId;
            RequestedUserId = requestedUserId;
            AmountToRequest = amountToRequest;
        }
        
    }

}

