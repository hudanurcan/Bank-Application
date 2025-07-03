
using Microsoft.AspNetCore.Mvc;
using bankApp.DataAccess.Context;
using bankApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using bankApp.DTO;
using bankApp.Business.Concrete;
using Microsoft.AspNetCore.Authorization;  
using System.Security.Claims;
using bankApp.Business.Abstract;


// Bu sınıfın kullanım amacı;  Kullanıcıların kaydını almak ve gerekli doğrulamaları yapmaktır.
// Bu sınıf, gelen POST isteklerini alır ve veritabanına kullanıcı bilgilerini kaydeder.

// UserController sınıfı, Web API sınıfı olarak yazılmıştır çünkü burada amacımız,
// fronttan gelen HTTP isteklerine yanıt vermek ve bu istekleri işlemektir.
// API'ler, istemci front ile sunucu backend arasında veri iletimi sağlar.

// IActionResult, bir controller metodunun çalışmasının sonunda ne tür bir yanıt döneceğini belirtir

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly BankAppContext _context;
    private readonly ILogger<UserController> _logger; // Hata, uyarı gibi bilgileri loglamak için kullanılır.
    private readonly AccountService _accountService;
    private readonly IUserService _userService;
    public UserController(BankAppContext context, ILogger<UserController> logger, AccountService accountService, IUserService userService)
    {
        _context = context;
        _logger = logger;
        _accountService = accountService;
        _userService = userService;
    }
   
    // kullanıcının profil bilgilerini getirme endpoint'i
    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetUserProfile(string userId)
    {
        Console.WriteLine("Frontend'den alınan Kullanıcı ID: " + userId);  // **localStorage'dan alınan userId**

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not authenticated.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (user == null)
            return NotFound("User not found.");

        return Ok(new { user.Name, user.Surname, user.UserId, user.Tc, user.Phone, user.Address, user.Birthday, user.Email });
    }

    // kullanıcı kaydı oluşturma endpoint'i
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        // Verilerin eksik olup olmadığını kontrol eder
        if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest(new { message = "Ad, Soyad, Email ve Şifre gereklidir." });

        }

        // Kullanıcının yaşını hesaplar
        DateTime birthDate = model.Birthday;
        int age = DateTime.Now.Year - birthDate.Year;
        if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
        {
            age--; // Eğer bu yıl doğum günü geçtiyse, yaş bir yıl eksik olacaktır
        }

        // 18 yaşından küçükse kayıt yapılmasına izin vermez
        if (age < 18)
        {
            return BadRequest(new { message = "18 yaşından küçük kullanıcılar kayıt olamaz." });
        }


        // TC kimlik numarasının varlığını kontrol eder
        var isUserExist = await _userService.IsTcExist(model.Tc);

        if (isUserExist)
        {
            return BadRequest(new { message = "Bu TC kimlik numarasına sahip bir kullanıcı zaten kayıtlı." });
        }

        // E-posta zaten mevcut mu diye kontrol eder
        var isEmailExist = await _userService.IsEmailExist(model.Email);
        if (isEmailExist)
        {
            return BadRequest(new { message = "Bu e-posta adresi ile zaten bir kullanıcı kaydedilmiş." });
        }

        // Telefon numarasının varlığını kontrol eder
        var isPhoneExist = await _userService.IsPhoneExist(model.Phone);
        if (isPhoneExist)
        {
            return BadRequest(new { message = "Bu telefon numarası ile zaten bir kullanıcı kaydedilmiş." });
        }

        // Şifreyi hash'ler
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Kullanıcıyı oluşturur
        var user = new User
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            Tc = model.Tc,
            Phone = model.Phone,
            Address = model.Address,
            Birthday = model.Birthday
        };

        // Şifreyi oluşturur ve kullanıcının ID'si ile ilişkilendirir
        var password = new Password
        {
            PasswordHash = passwordHash,
            User = user
        };

        // Kullanıcıyı ve şifreyi veritabanına kaydeder
        _context.Users.Add(user);
        _context.Passwords.Add(password);
        await _context.SaveChangesAsync();  // Değişiklikleri veritabanına kaydeder

        // Hesap oluştur 
        var accountDto = new RegisterDto
        {
            UserId = user.UserId,  // Kullanıcı ID'sini RegisterDto'ya ekler
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            Password = model.Password,
            Tc = model.Tc,
            Phone = model.Phone,
            Address = model.Address,
            Birthday = model.Birthday
        };

        await _accountService.CreateAccountAsync(accountDto);  // Hesap oluşturur

        return Ok(new { message = "Kayıt başarılı ve hesap oluşturuldu." });
    }
}


