using bankApp.DataAccess.Context;
using bankApp.DTO;
using bankApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Bu TransactionService sınıfı, bankacılık işlemleri ve para transferleri gibi işlevleri yönetir. Para transferi gerçekleştiren bir sınıftır.
// Bu sınıfın amacı, bir kullanıcının başka bir kullanıcıya para göndermesini sağlamak ve işlem sonrası bakiyeleri güncellemektir.

namespace bankApp.Business.Concrete
{
    public class TransactionService
    {
        private readonly BankAppContext _context;

        public TransactionService(BankAppContext context)
        {
            _context = context;
        }

        // para transfer metodu
        public bool TransferMoney(TransactionDto transactionDto)
        {
            Console.WriteLine($"Gelen senderUserId: {transactionDto.SenderUserId}");

            // Gönderen hesabı alır
            var senderAccount = _context.BankAccounts
                .FirstOrDefault(acc => acc.UserId == transactionDto.SenderUserId); 
            // Bu sorgu, transactionDto içinde belirtilen SenderUserId'ye sahip olan ilk banka hesabını bulur.


            // Alıcı hesabı alır
            var receiverAccount = _context.BankAccounts
                .FirstOrDefault(acc => acc.Iban == transactionDto.ReceiverIban);

            if (senderAccount == null || receiverAccount == null)
            {
                return false; 
            }
            // Para transferi öncesi bakiyeleri kontrol eder
            Console.WriteLine($"Gönderen Hesap Bakiyesi (Önce): {senderAccount.Balance}");
            Console.WriteLine($"Alıcı Hesap Bakiyesi (Önce): {receiverAccount.Balance}");

            if (senderAccount.Balance < transactionDto.Amount)
            {
                return false; // Yetersiz bakiye
            }

            // Para transferini yapar (Gönderenin bakiyesinden düşer ve alıcının bakiyesi artar)
            senderAccount.Balance -= transactionDto.Amount;
            receiverAccount.Balance += transactionDto.Amount;

            // Para transferi sonrası bakiyeleri kontrol eder
            Console.WriteLine($"Gönderen Hesap Bakiyesi (Sonra): {senderAccount.Balance}");
            Console.WriteLine($"Alıcı Hesap Bakiyesi (Sonra): {receiverAccount.Balance}");

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }

            return true; // Transfer tamamlandı
        }
    }
}
