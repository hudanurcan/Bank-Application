using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bankApp.DTO;
using bankApp.Entity.Entities;

namespace bankApp.Business.Abstract
{
    public interface IUserService
    {
        Task<User> LoginAsync(string tc, string password);
        //  Bu metot, kullanıcının giriş yapması için gerekli olan TC kimlik numarası (tc) ve şifresini (password) alır.
        //  Eğer doğruysa, başarılı bir User (kullanıcı) nesnesi döndürecektir. Bu işlem asenkron bir işlem olduğu için Task döndürür.
        //  Bu metodun geri dönüş tipi, User nesnesidir, ancak işlem asenkron olduğu için Task tipinde döner.

        Task<bool> IsTcExist(string tc); // Bu metot, verilen TC kimlik numarasının veritabanında kayıtlı olup olmadığını kontrol eder.
        Task<bool> IsEmailExist(string email);  // Bu metot, verilen email in veritabanında kayıtlı olup olmadığını kontrol eder.

        Task<bool> IsPhoneExist(string phone); // Bu metot, verilen telefon numarasının veritabanında kayıtlı olup olmadığını kontrol eder.

    }
}
