using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bankApp.Entity.Entities;

// Bu IUserDal arayüzü, kullanıcı verilerini veritabanından almak için gerekli metodları tanımlar.
// Amacı, kullanıcı bilgileriyle ilgili işlemleri soyutlamak ve bu işlemleri gerçekleştirecek somut sınıflara bir şablon sunmaktır.

namespace bankApp.DataAccess.Abstract
{
    public interface IUserDal
    {
        Task<User> GetUserByTCAsync(string tc); // TC kimlik numarasına göre veritabanından bir kullanıcıyı asenkron olarak getirir.
                                                // Kullanıcının veritabanında var olup olmadığını kontrol etmek için kullanılır.
        Task<User> GetUserByEmailAsync(string email); // email e göre veritabanından bir kullanıcıyı asenkron olarak getirir.
                                                      // Kullanıcının veritabanında var olup olmadığını kontrol etmek için kullanılır.
        Task<User> GetUserByPhoneAsync(string phone);  // Telefon numarasına göre veritabanından bir kullanıcıyı asenkron olarak getirir.
                                                       // Kullanıcının veritabanında var olup olmadığını kontrol etmek için kullanılır.

    }
}
