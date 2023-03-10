
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
        public string UsernameFrom { get; set; }
        public string UsernameTo { get; set; }

        public string UsernameToFrom {
            get
            {
                if (TransferTypeId == 1) return UsernameFrom;
                else if (TransferTypeId == 2) return UsernameTo;
                else return "Unknown";
            }
        }

        public string TransferTypeName
        {
            get
            {
                if (TransferTypeId == 1) return "Request";
                else if (TransferTypeId == 2) return "Send";
                else return "Unknown";
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
}
