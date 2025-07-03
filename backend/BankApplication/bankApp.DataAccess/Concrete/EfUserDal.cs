using bankApp.DataAccess.Abstract;
using bankApp.DataAccess.Context;
using bankApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// EfUserDal sınıfı, Entity Framework kullanarak veritabanı işlemleri gerçekleştiren bir sınıftır.
// Bu sınıf, IUserDal arayüzünü implement eder ve kullanıcı verileri üzerinde veritabanı işlemleri yapar.
// Özellikle kullanıcıyla ilgili bilgileri getirme (fetching) işlemleriyle ilgilidir.

// EfUserDal sınıfı veritabanına erişim sağlar ve kullanıcı bilgilerini almak için async (asenkron) metotlar sunar.
// Bu sınıf veritabanı erişimini soyutlar ve kullanıcı ile ilgili veritabanı işlemlerini bu sınıfta toplar.


// Bu sınıfın amacı, kullanıcı verilerini veritabanından almak ve kullanıcıya dair işlemleri yapmak.

namespace bankApp.DataAccess.Concrete
{
    public class EfUserDal : IUserDal
    {
        private readonly BankAppContext _context;

        public EfUserDal(BankAppContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByTCAsync(string tc) // TC numarasına göre kullanıcıyı veritabanından asenkron olarak getirir.
        {
            return await _context.Users // Users tablosu üzerinde işlem yapar
                .Include(u => u.Password)  // Her kullanıcıyla ilişkili olan şifre bilgilerini de dahil eder
                .FirstOrDefaultAsync(u => u.Tc == tc); // ilk eşleşmeyi getirir.
        }

        public async Task<User> GetUserByEmailAsync(string email) // email e göre kullanıcıyı veritabanından asenkron olarak getirir.
        {
            return await _context.Users  // Users tablosu üzerinde işlem yapar
                                 .Include(u => u.Password) // Her kullanıcıyla ilişkili olan şifre bilgilerini de dahil eder
                                 .FirstOrDefaultAsync(u => u.Email == email); // ilk eşleşmeyi getirir.
        }

        public async Task<User> GetUserByPhoneAsync(string phone) // Telefon numarasına göre kullanıcıyı veritabanından asenkron olarak getirir.
        {
            return await _context.Users // Users tablosu üzerinde işlem yapar
                                 .Include(u => u.Password) // Her kullanıcıyla ilişkili olan şifre bilgilerini de dahil eder
                                 .FirstOrDefaultAsync(u => u.Phone == phone);  // ilk eşleşmeyi getirir.
        }
    }
}
