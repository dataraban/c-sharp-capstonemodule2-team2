
namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
        public string UsernameFrom { get; set; }
        public string UsernameTo { get; set; }

        public string UsernameToFrom {
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
                if (TransferTypeID == 1) return "Request";
                else if (TransferTypeID == 2) return "Send";
                else return "Unknown";
            }
        }
    }
}
