
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

    public class PastTransfer
    {
        public int TransferId { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }

        public string UsernameTo { get; set; }

        public string UsernameFrom { get; set; }

        public decimal Amount { get; set; }

        public string UsernameToFrom
        {
            get
            {
                if (TransferTypeID == 1) return UsernameFrom;
                else if (TransferTypeID == 2) return UsernameTo;
                else return "Unknown";
            }
        }
        public string TransferTypeName
        {
            get
            {
                if (TransferTypeID == 1) return "From";
                else if (TransferTypeID == 2) return "To";
                else return "Unknown";
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

}

