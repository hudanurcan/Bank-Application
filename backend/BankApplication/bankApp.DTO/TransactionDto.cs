using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.DTO
{
    public class TransactionDto
    {
        public int SenderUserId { get; set; }  
        public string ReceiverIban { get; set; } 
        public decimal Amount { get; set; } // Transfer edilen miktar
        public string? Description { get; set; } 

    }
}
