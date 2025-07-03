using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.Entity.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SenderUserId { get; set; }
        public int? ReceiverUserId { get; set; }  
                                                 
        public string? ReceiverIban { get; set; }  
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public TransactionType TransactionType { get; set; }  

        public string SenderName { get; set; }  
        public string? ReceiverName { get; set; }  

    }
}
