using bankApp.Business.Abstract;
using bankApp.DataAccess.Abstract;
using bankApp.DTO;
using bankApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

// Bu UserManager sınıfı, kullanıcı yönetimi ve doğrulama işlemleri için kullanılır.
// Temelde, kullanıcıları kaydetme, login işlemleri, kullanıcı bilgisi kontrolü gibi işlemleri yönetir.
// Sınıf, kullanıcıyla ilgili işlemler için IUserDal adlı bir veri erişim katmanı kullanır.

namespace bankApp.Business.Concrete
{
    public class UserManager : IUserService
    //UserManager sınıfı IUserService arayüzünü implement ediyor. Yani, UserManager, IUserService'in içinde tanımlanmış olan metotları yerine getirecek.
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        // tc kontrolü yapar
        public async Task<bool> IsTcExist(string tc)
            // bu TC numarasına sahip bir kullanıcının veritabanında olup olmadığını kontrol eder. varsa user değişkenine atanır
        {
            var user = await _userDal.GetUserByTCAsync(tc);
            return user != null;
        }

        // email kontrolü yapar
        public async Task<bool> IsEmailExist(string email)
        {
            var user = await _userDal.GetUserByEmailAsync(email); // Veritabanında email'i arar
            return user != null;
        }

        // tel no kontrolü yapar
        public async Task<bool> IsPhoneExist(string phone)
        {
            var user = await _userDal.GetUserByPhoneAsync(phone); // Veritabanında telefon numarasını kontrol eder
            return user != null;
        }

        public async Task<User> LoginAsync(string tc, string password)
        // Bu metot, tc ve password parametrelerini alarak, bir kullanıcının sisteme giriş yapıp yapamayacağını kontrol eder.
        // Eğer kullanıcı bulunursa ve şifre doğruysa, kullanıcıyı geri döndürür. Aksi takdirde, giriş başarısız olur ve null döndürülür.


        {
            var user = await _userDal.GetUserByTCAsync(tc);
            if (user == null)
                return null;


            if (BCrypt.Net.BCrypt.Verify(password, user.Password.PasswordHash))
                // kullanıcının girdiği şifre ile veri tabanındaki şifre hash'ini karşılaştırılır
                return user;

            return null; // şifre yanlışsa null döner.
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        // Bu metodun amacı, girilen şifrenin hash değerini hesaplayarak, bunun daha önce saklanan hash değeriyle karşılaştırmaktır.
        // Eğer her iki hash değeri eşleşiyorsa, şifre doğrudur. Aksi takdirde, şifre yanlıştır.
        // storedHash -> veritabanında saklanan hash değeridir
        {
            using (var sha256 = SHA256.Create()) // SHA256 (Secure Hash Algorithm 256-bit), şifreleme algoritmasıdır.
             // using anahtar kelimesi, sha256 nesnesi ile yapılan işlemler tamamlandıktan sonra nesnenin bellekten temizlenmesini sağlar.
             // Bu, kaynakların verimli bir şekilde yönetilmesi için önemlidir.


            {
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // computedHash byte dizisi, Base64 formatına dönüştürülür. Base64, byte dizilerini güvenli bir şekilde metne dönüştürmek için kullanılan bir formattır.
                var computedHashString = Convert.ToBase64String(computedHash);
                // computedHashString -> Şifrenin hash'lenmiş halinin Base64 formatındaki string karşılığıdır.

                return computedHashString == storedHash;
                // Burada, computedHashString (yeni hesaplanan hash değeri) ile storedHash karşılaştırılır.
                // Eğer iki hash değeri eşleşirse, true döndürülür; yani, girilen şifre doğru olmuştur.
            }
        }
    }
}
