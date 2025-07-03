using bankApp.Business.Abstract;
using bankApp.Business.Concrete;
using bankApp.DataAccess.Context;
using bankApp.DTO;
using bankApp.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// AccountController kullanıcının banka hesabıyla ilgili işlemleri gerçekleştiren API endpoint'lerini içeren bir controller'dır.
// Controller'lar HTTP isteklerini yönlendiren sınıflardır
// asenkron çalışma , bir işlem bitene kadar başka işlemler yapılabilir demektir


namespace bankApp.API.Controllers
{
    [Route("api/[controller]")] 
    //[controller] -> sınıf adının (Controller'dan önceki kısmı) otomatik olarak eklenmesini sağlar.Yani api/account gibi bir yol olur.
    [ApiController] //controller'ın API için uygun olduğunu belirtir ve gelen isteklerin doğrulanmasını otomatikleştirir.
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly BankAppContext _context;  

        public AccountController(AccountService accountService, BankAppContext context)
        {
            _accountService = accountService;
            _context = context;  

        }

        // Bakiye'yi getirme endpoint'i
        [HttpGet("get-balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        // Bu metod bir Task döner, çünkü asenkron bir metoddur. IActionResult, bu metodun geri döndüreceği yanıtın tipini belirtir.
        // IActionResult, HTTP yanıtını temsil eden genel bir tiptir ve dönecek sonucu belirtir.
        {
            var userAccount = await _accountService.GetBalanceByUserIdAsync(userId);
            // await -> Bu, asenkron işlemi beklememizi sağlar.
            // Yani, _accountService.GetBalanceByUserIdAsync(userId) metodu çalışırken, sistem başka işlere de devam edebilir.

            if (userAccount == null)
            {
                return NotFound("Hesap bulunamadı.");
            }

            return Ok(userAccount.Balance); // Kullanıcının bakiyesini döndürüyoruz
        }

        // Hesap oluşturma endpoint'i
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterDto registerDto)
        // [FromBody] RegisterDto registerDto -> Burada, HTTP POST isteği ile gelen RegisterDto tipindeki veriler body'den alınır.
        // Bu, kullanıcının kayıt bilgilerini içeren bir veri transfer nesnesidir.
        // Yani, istemci yeni hesap oluşturmak için gerekli bilgileri bu nesneyle gönderecektir.
        {
            if (registerDto == null)
            {
                return BadRequest("Invalid data.");
            }

            await _accountService.CreateAccountAsync(registerDto);
            return Ok(new { message = "Account successfully created." });
        }

        // Para yatırma endpoint'i
        [HttpPost("deposit/{userId}")]
        public async Task<IActionResult> Deposit(int userId, [FromBody] DepositRequest request)
        {
            try
            {
                var newBalance = await _accountService.DepositAsync(userId, request.Amount);
                // request.Amount -> Bu, para yatırılacak miktarı temsil eder. Kullanıcı bu miktarı DepositRequest üzerinden gönderir

                // Yeni işlem kaydını oluşturuyoruz
                var transaction = new Transaction
                // Transaction -> Para yatırma işleminin kaydını tutan işlem nesnesi'dir. Bu nesne, işlemin tüm bilgilerini içerir:
                {
                    SenderUserId = userId,
                    ReceiverIban = null,  // Bu kısmı NULL olarak bırakıyoruz çünkü yatırma işlemi alıcıya gerek duymuyor.
                    ReceiverName = null,
                    ReceiverUserId = userId,
                    SenderName = "Kendi Hesabı",
                    Amount = request.Amount,
                    Description = "Para yatırma işlemi",
                    DateOfTransaction = DateTime.Now,
                    TransactionType = TransactionType.AtmDeposit, // İşlem türünü Deposit olarak ayarlıyoruz
                    CreatedAt = DateTime.Now
                };

                _context.Transactions.Add(transaction); // yeni oluşturulan transaction nesnesi veritabanına eklenir.
                await _context.SaveChangesAsync(); //  Veritabanındaki değişikliklerin asenkron olarak kaydedilmesini sağlar.

                return Ok(newBalance); // Yeni bakiyeyi döndürüyoruz
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Para çekme endpoint'i
        [HttpPost("withdraw/{userId}")]
        public async Task<IActionResult> Withdraw(int userId, [FromBody] WithdrawRequest request)
        {
            try
            {
                var newBalance = await _accountService.WithdrawAsync(userId, request.Amount);

                // Yeni işlem kaydını oluşturuyoruz
                var transaction = new Transaction
                {
                    SenderUserId = userId,
                    ReceiverUserId = userId,
                    Amount = request.Amount,
                    ReceiverName = null,
                    SenderName = "Kendi Hesabı",
                    Description = "Para çekme işlemi",
                    DateOfTransaction = DateTime.Now,
                    TransactionType = TransactionType.AtmWithdraw, // İşlem türünü Withdraw olarak ayarlıyoruz
                    CreatedAt = DateTime.Now
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok(newBalance); // Yeni bakiyeyi döndürüyoruz
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-cards/{userId}")]
        public async Task<IActionResult> GetBankAccounts(int userId)
        {
            var accounts = await _accountService.GetBankAccountsAsync(userId); // kullanıcının banka kartlarına ait verileri alır
            if (accounts == null)
            {
                return NotFound("Kart bilgisi bulunamadı");
            }
            return Ok(accounts);
        }

        // Hesap Kapatma endpoint'i
        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> DeactivateAccount(int userId)
        {
            try
            {
                // Hesap kapama işlemi
                await _accountService.DeactivateAccountByUserId(userId); // kullanıcının hesabını siler
                return Ok(new { message = "Hesabınız başarıyla kapatıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
    }
}
