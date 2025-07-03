using bankApp.DataAccess.Abstract;
using bankApp.DataAccess.Context;
using bankApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Bu sınıf, BankAccountDal, IBankAccountDal arayüzünü gerçekleştiren (implement eden) bir concrete sınıf'tır.
// Bu sınıfın amacı, BankAccount (banka hesapları) ile ilgili veritabanı işlemlerini gerçekleştirmek ve veritabanına erişim sağlamaktır.
// Yani, veritabanıyla doğrudan iletişim kuran ve işlemleri gerçekleştiren bir sınıftır.

// IBankAccountDal arayüzünü implement ederek, banka hesaplarına yönelik işlemleri soyutlar.
// Bu sayede veritabanı bağımsız hale gelir ve ileride veritabanı değişikliği yapılması gerektiğinde sadece bu sınıfı değiştirmek yeterli olur.

namespace bankApp.DataAccess.Concrete
{
    public class BankAccountDal : IBankAccountDal
    {
        private readonly BankAppContext _context;

        public BankAccountDal(BankAppContext context)
        {
            _context = context;
        }

        public void Add(BankAccount account) // Verilen BankAccount nesnesini veritabanına ekler.
        {
            _context.BankAccounts.Add(account);
            _context.SaveChanges();
        }

        public List<BankAccount> GetAll() // Veritabanındaki tüm banka hesaplarını alır ve bir liste olarak döndürür.
        {
            return _context.BankAccounts.ToList();
        }
    }

}
