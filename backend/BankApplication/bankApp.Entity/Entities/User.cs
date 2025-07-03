using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.Entity.Entities
{
    public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Tc { get; set; }
    public DateTime Birthday { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public ICollection<BankAccount> BankAccounts { get; set; } // Kullanıcının birden fazla hesabı olabilir


    public virtual Password Password { get; set; }
}

}
