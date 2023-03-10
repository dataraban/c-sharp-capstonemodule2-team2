
namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public int TransferStatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

    }

    public class SendTransfer
    {
        public int UserIdFrom {get; set;}
        public int UserIdTo { get; set; }
        public decimal AmountToSend { get; set; }

    }


    public class ReceiveTransfer
    {
        public int RequestingUserId { get; set; }
        public int RequestedUserId { get; set; }
        public decimal AmountToRequest { get; set; }

    }
}
