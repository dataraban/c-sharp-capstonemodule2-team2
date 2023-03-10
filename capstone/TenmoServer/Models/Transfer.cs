
using System;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "UserID cannot be blank")]
        public int UserIdFrom {get; set;}

        [Required(ErrorMessage = "UserID cannot be blank")]
        public int UserIdTo { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Amount cannot be blank or less than $0")]
        public decimal AmountToSend { get; set; }

    }


    public class ReceiveTransfer
    {
        [Required(ErrorMessage = "UserID cannot be blank")]
        public int RequestingUserId { get; set; }

        [Required(ErrorMessage = "UserID cannot be blank")]
        public int RequestedUserId { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Amount cannot be blank or less than $0")]
        public decimal AmountToRequest { get; set; }

    }
}
