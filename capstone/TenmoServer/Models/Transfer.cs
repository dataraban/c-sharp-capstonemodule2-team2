
namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeID { get; set; }
        public int TransferStatusID { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

    }

    public class SendTransfer
    {
        public string UsernameFrom {get; set;}
        public string UsernameTo { get; set; }
        public decimal AmountToSend { get; set; }

    }
}
