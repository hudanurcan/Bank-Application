using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.DTO
{
    public class WithdrawRequest
    {
        public decimal Amount { get; set; }  // Çekilecek tutar
        public int UserId { get; set; }     
    }
}
