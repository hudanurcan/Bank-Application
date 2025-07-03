using bankApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Bu interface, bankadaki hesapla ilgili yapılması gereken işlemleri tanımlar.

namespace bankApp.Business.Abstract
{
    public interface IBankAccountService
    {
        void CreateAccount(int userId); // Bu metot, kullanıcının kimliğini temsil eden bir userId parametresi alır.
        List<BankAccount> GetAllAccounts(); // Bu metot, sistemdeki tüm banka hesaplarını listelemeye yarar.
    }
}
