using bankApp.Business.Abstract;
using bankApp.Business.Concrete;
using bankApp.DataAccess.Context;
using bankApp.DTO;
using bankApp.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// TransactionController -> Banka işlemlerini yöneten bir API kontrolcüsüdür.
// Banka işlemleriyle (para transferi, para yatırma, para çekme, işlem geçmişi görüntüleme) ilgili API endpoint'lerini içerir.
// Bu sınıfın amacı, kullanıcılara para transferi yapma, işlem geçmişini görüntüleme gibi işlemleri sunmaktır.


// LINQ (Language Integrated Query), .NET platformunda veri kaynaklarına sorgular yazmak için kullanılan bir query language'dir,
// C#, VB.NET ve diğer .NET dillerine entegre edilmiştir.
// LINQ, veri üzerinde sorgular yazmayı daha kolay, anlaşılır ve güçlü hale getirir.
// Veri kaynakları, SQL veritabanları, XML, koleksiyonlar, diziler, vb. olabilir.
// doğrudan C# kodu içinde kullanılır
namespace bankApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly BankAppContext _context; // Entity Framework Core üzerinden veritabanı işlemleri yapmak için kullanılır.

        private readonly TransactionService _transactionService; // İşlem mantığı ve işlemlerle ilgili işlemleri yapacak sınıf.

        public TransactionController(TransactionService transactionService, BankAppContext context)
        {
            _transactionService = transactionService;
            _context = context;

        }


        [Authorize] // [Authorize]: Bu, yalnızca giriş yapmış ve yetkilendirilmiş kullanıcıların bu endpoint'i kullanabileceği anlamına gelir.
                    // Yani sadece doğru kimlik doğrulama yapan kişiler bu işlemi gerçekleştirebilir.

        // para transfer endpointi
        [HttpPost("transfer")]
        public ActionResult TransferMoney([FromBody] TransactionDto transactionDto)
        {
            Console.WriteLine($"Gelen transfer verisi: {transactionDto.SenderUserId}, {transactionDto.ReceiverIban}, {transactionDto.Amount}, {transactionDto.Description}");

            // Eğer SenderUserId boşsa, hata döndürüyoruz
            if (string.IsNullOrEmpty(transactionDto.SenderUserId.ToString()))
            { // null ya da boş olup olmadığını kontrol eder
                return BadRequest("SenderUserId eksik.");
            }
            if (transactionDto.SenderUserId == 0)
            { // 0'ın geçerli bir ID olup olmadığına bakar
                return BadRequest("Geçersiz SenderUserId");
            }


            // Kullanıcının bank hesabını alıyoruz ve bakiyesini kontrol ediyoruz
            var senderAccount = _context.BankAccounts.FirstOrDefault(a => a.UserId == transactionDto.SenderUserId);
            // FirstOrDefault metodu, verilen şartı sağlayan ilk öğeyi döndürür. Eğer böyle bir öğe bulunmazsa, null döner.
            // Bu değişken, gönderen kullanıcının banka hesabı bilgisini tutar

            var receiverAccount = _context.BankAccounts.FirstOrDefault(a => a.Iban == transactionDto.ReceiverIban);
            // ReceiverIban, transactionDto üzerinden gelen alıcı IBAN numarasıdır.
            // receiverAccount: Bu değişken, alıcı kullanıcının banka hesabı bilgisini tutar.

            if (senderAccount == null)
            {
                return NotFound(new { message = "Kullanıcı hesabı bulunamadı." });
            }

            if (receiverAccount == null )
            {
                return BadRequest(new { message = "Geçersiz Iban Bilgisi" });
            }

            var userBalance = senderAccount.Balance; // senderAccount'daki bakiyeyi tutar

            // Eğer kullanıcının bakiyesi yetersizse hata dönüyoruz
            if (userBalance < transactionDto.Amount)
            {
                return BadRequest(new { message = "Yetersiz bakiye." });
            }
            Console.WriteLine($"Frontend'den alınan Kullanıcı ID: {transactionDto.SenderUserId}");

            if (transactionDto.Amount <= 0)
            {
                return BadRequest(new { message = "Göndermek istediğiniz tutar geçersiz." });


            }

            var senderUser = _context.Users.FirstOrDefault(u => u.UserId == transactionDto.SenderUserId);
            // senderUser -> Gönderen kullanıcıyı alır, yani SenderUserId ile eşleşen kullanıcıyı alır.

            var receiverUser = _context.Users.FirstOrDefault(u => u.UserId == receiverAccount.UserId); // Alıcıyı BankAccount'tan alır
            // Alıcıyı alır, yani receiverAccount.UserId ile eşleşen kullanıcıyı.
            // Burada alıcı, BankAccounts tablosundan alındığı için, alıcının UserId'si receiverAccount.UserId ile alınır.


            //if (senderUser == null || receiverUser == null)
            //{
            //    return NotFound("Kullanıcı bilgileri eksik.");
            //}

            // Gönderen ve alıcı isimlerini alır
            var senderName = senderUser.Name + " " + senderUser.Surname;
            var receiverName = receiverUser.Name + " " + receiverUser.Surname;

            //// Para transferini yap
            //senderAccount.Balance -= transactionDto.Amount;
            //receiverAccount.Balance += transactionDto.Amount;

            // İşlem tarihlerini günceller
            var transactionDate = DateTime.Now;


            bool isSuccess = _transactionService.TransferMoney(transactionDto);
            // _transactionService.TransferMoney(transactionDto) -> Burada, TransferMoney metodu çağrılır.
            // Bu metod, business katmanı içinde para transferi işlemini yapar.

            if (isSuccess)
            {

               
                var senderTransaction = new Transaction  // Gönderenin işlemi ile ilgili bir Transaction nesnesi oluşturulur.
                {
                    SenderUserId = transactionDto.SenderUserId,
                    ReceiverIban = transactionDto.ReceiverIban,
                    Amount = transactionDto.Amount,
                    Description = transactionDto.Description,
                    DateOfTransaction = DateTime.Now,
                    TransactionType = TransactionType.TransferOut,  // TransferOut (Gönderme)
                    CreatedAt = DateTime.Now,
                    SenderName = senderName,   
                    ReceiverName = receiverName 
                };

                _context.Transactions.Add(senderTransaction);

                // Alıcının işlemi ile ilgili bir Transaction nesnesi oluşturulur.
                var receiverTransaction = new Transaction
                {
                    SenderUserId = transactionDto.SenderUserId,
                    ReceiverIban = transactionDto.ReceiverIban,
                    Amount = transactionDto.Amount,
                    Description = transactionDto.Description,
                    DateOfTransaction = DateTime.Now,
                    TransactionType = TransactionType.TransferIn,  // TransferIn (Alma)
                    CreatedAt = DateTime.Now,
                    SenderName = senderName,  
                    ReceiverName = receiverName 
                };

                _context.Transactions.Add(receiverTransaction);
                _context.SaveChanges();

                //return Ok(new { message = "Transfer başarılı!" });
                return Ok(new
                {
                    message = "Transfer başarılı!",
                    updatedBalance = senderAccount.Balance // Güncel bakiyeyi de gönderir
                });
            }

            return BadRequest("Transfer işlemi başarısız.");
        }


        
        // işlem geçmişi endpoint'i
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetTransactionHistory(int userId) 
        {
            var transactions = await _context.Transactions // Transactions tablosu, veritabanındaki işlemleri temsil eder.
                                                           // _context, Entity Framework Core'un veritabanı erişim nesnesidir.

                //  Where -> Bu, LINQ sorgusunun koşuludur. Transactions tablosundaki tüm işlemler üzerinden filtreleme yapılır.
                //  Şu koşullara göre işlem türleri sorgulanır:
                .Where(t =>
                    // Kullanıcı tarafından gönderilen işlemler (Para gönderme işlemi: TransferOut)
                    t.SenderUserId == userId && t.TransactionType == TransactionType.TransferOut ||

                    // Kullanıcıya gönderilen işlemler (Para alma işlemi: TransferIn)
                    t.ReceiverIban != null && t.TransactionType == TransactionType.TransferIn && _context.BankAccounts.Any(b => b.UserId == userId && b.Iban == t.ReceiverIban) ||

                    // Kullanıcının ATM'ye para yatırma işlemi
                    t.TransactionType == TransactionType.AtmDeposit && t.SenderUserId == userId ||

                    // Kullanıcının ATM'den para çekme işlemi
                    t.TransactionType == TransactionType.AtmWithdraw && t.SenderUserId == userId
                )
                .OrderByDescending(t => t.DateOfTransaction)  // En son yapılan işlemi önce göstermek için sıralama
                .ToListAsync();

            // Eğer işlem geçmişi bulunamazsa
            if (transactions == null || !transactions.Any())
            {
                return NotFound("No transactions found.");
            }

            // Geçerli işlemleri başarılı şekilde döndürür
            return Ok(transactions);
        }

    }
}
