using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.Entity.Entities
{
    public class BankAccount
    {

        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public string Iban { get; set; }
        public string CardNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } 
        public string CustomerNo { get; set; }  
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
