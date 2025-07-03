using bankApp.DataAccess.Context;
using bankApp.DTO;
using bankApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// AccountService sınıfı, banka hesaplarına dair işlemleri yönetmek için kullanılan bir servis sınıfıdır.
// Bu sınıfın temel amacı, kullanıcıların banka hesapları üzerinde gerçekleştirilecek işlemleri iş mantığı olarak yönetmektir.
// Bu sınıf, banka hesaplarıyla ilgili işlemleri veritabanına yazma, okuma ve güncelleme işlemleri için Entity Framework'ü kullanır.


namespace bankApp.Business.Concrete
{
    public class AccountService
    {
        private readonly BankAppContext _context;

        public AccountService(BankAppContext context)
        {
            _context = context;
        }


        public async Task<BankAccount> GetBalanceByUserIdAsync(int userId)
        // belirli bir kullanıcıya ait banka hesabını veritabanından sorgular ve o kullanıcıya ait hesap bilgilerini döndürür
        // userId parametresini alarak, o kullanıcıya ait banka hesabı bilgilerini BankAccount nesnesi olarak döndürür.

        {

            var account = await _context.BankAccounts
                .Where(a => a.UserId == userId) // BankAccounts tablosunda yer alan tüm banka hesapları içerisinden, userId'ye sahip olanları sorgular.
                .FirstOrDefaultAsync();  // İlk eşleşen BankAccount'u alır

            if (account == null)
            {
                return null;  // Hesap bulunamadı
            }

            return account;  
        }

        // Para yatırma işlemi
        public async Task<decimal> DepositAsync(int userId, decimal amount) // decimal değeri döndürür
                                                                            // decimal amount -> kullanıcının yatırmak istediği miktarı belirtir.
        {
            var account = await GetBalanceByUserIdAsync(userId); // userId'ye göre kullanıcının banka hesabı GetBalanceByUserIdAsync metodu ile alınır.
            
            if (account == null)
            {
                throw new Exception("Hesap bulunamadı.");
            }

            if (amount <= 0)
            {
                throw new Exception("Yatırılacak miktar sıfırdan büyük olmalıdır.");
            }

            account.Balance += amount;
            await _context.SaveChangesAsync();  
            return account.Balance;  // Güncellenmiş bakiyeyi döndürür
        }

        // Para çekme işlemi
        public async Task<decimal> WithdrawAsync(int userId, decimal amount)
        {
            var account = await GetBalanceByUserIdAsync(userId); // userId'ye göre kullanıcının banka hesabı GetBalanceByUserIdAsync metodu ile alınır.
            if (account == null)
            {
                throw new Exception("Hesap bulunamadı.");
            }

            if (amount <= 0)
            {
                throw new Exception("Çekilecek miktar sıfırdan büyük olmalıdır.");
            }

            if (account.Balance < amount)
            {
                throw new Exception("Bakiyeniz yetersiz.");
            }

            account.Balance -= amount;
            await _context.SaveChangesAsync();  
            return account.Balance;   // Güncellenmiş bakiyeyi döndürür
        }

        public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync(int userId)
        // Bu metodun amacı, belirli bir kullanıcı ID'sine sahip olan banka hesaplarını veritabanından alıp döndürmektir. liste döndürür
        {
            return await _context.BankAccounts.Where(b => b.UserId == userId).ToListAsync();
            // ToListAsync(), veritabanına sorgu gönderip, gelen verileri liste formatında döndüren bir metottur.

        }

        // Hesap oluşturma metodu
        public async Task CreateAccountAsync(RegisterDto registerDto)
        {
            // Kart numarasını oluşturur
            var cardNumber = GenerateCardNumber();

            // Müşteri numarasını oluşturur.
            var customerNo = GenerateCustomerNo();


            var account = new BankAccount
            {
                UserId = registerDto.UserId,  
                Balance = 0.0m,  // Hesap bakiyesi başlangıçta 0
                Iban = GenerateIban(),  
                CardNumber = cardNumber,  
                CustomerNo = customerNo,  
                CreatedAt = DateTime.Now,  // Hesap oluşturulma tarihi
                UpdatedAt = DateTime.Now,  // Hesap güncellenme tarihi
                ExpiryDate = DateTime.Now.AddYears(5),  // SKT: Hesap oluşturulma tarihinden 5 yıl sonrası
                IsActive = true  // Hesap aktif mi
            };

            // Hesap veritabanına eklenir
            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();  
        }

        // Kart numarasını rastgele üretme fonksiyonu
        private string GenerateCardNumber()
        {
            Random random = new Random();
            string cardNumber = "";

            bool isUnique = false;

            // Benzersiz kart numarası oluşturulana kadar döngüye girer
            while (!isUnique)
            {
                cardNumber = "";

                // 12 haneli rastgele kart numarası oluşturur
                for (int i = 0; i < 16; i++)
                {
                    cardNumber += random.Next(0, 10).ToString();  
                }

                // Veritabanında bu kart numarasının olup olmadığını kontrol eder
                isUnique = !_context.BankAccounts.Any(b => b.CardNumber == cardNumber);
            }

            return cardNumber;
        }
        private string GenerateCustomerNo()
        {
            Random random = new Random();
            string customerNo = "";
            bool isUnique = false;

            while (!isUnique)
            {
                customerNo = "";

                
                for (int i = 0; i < 12; i++)
                {
                    customerNo += random.Next(0, 10).ToString();  
                }


                // Veritabanında bu müşteri numarasının olup olmadığını kontrol eder
                isUnique = !_context.BankAccounts.Any(b => b.CustomerNo == customerNo);
            }

            return customerNo;
        }


        // Iban numarası üretme fonksiyonu
        private string GenerateIban()
        {
            Random random = new Random();
            string iban = "TR"; 
            bool isUnique = false;

            while (!isUnique)
            {

                for (int i = 0; i < 24; i++)
                {

                    iban += random.Next(0, 10).ToString();  
                }

                isUnique = !_context.BankAccounts.Any(b => b.Iban == iban);
            }

            return iban;
        }


        // Hesap silme metodu
        public async Task DeactivateAccountByUserId(int userId)
        {
            // Kullanıcının aktif olan hesabını buluyoruz
            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsActive == true);

            if (account == null)
            {
                throw new Exception("Aktif hesap bulunamadı.");
            }

            // Hesap aktifse, kapatıyoruz
            account.IsActive = false; // isActive kısmı false olarak günceller
            account.UpdatedAt = DateTime.Now;  // Hesap güncellenme tarihi
            _context.BankAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        // Kullanıcının hesabının aktif olup olmadığını kontrol etme metodu
        public async Task<bool> IsAccountActiveAsync(int userId)
        {
            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return account != null && account.IsActive;
        }
    }

}
