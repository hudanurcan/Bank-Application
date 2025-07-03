using bankApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// bu sınıf banka hesabı ile ilgili temel veri erişim işlemleri (ekleme ve listeleme) için bir plan sunar.

namespace bankApp.DataAccess.Abstract
{
    public interface IBankAccountDal
    {
        void Add(BankAccount account); // banka hesabını veritabanına ekler
        List<BankAccount> GetAll(); // banka hesaplarını listeler
    }
}
