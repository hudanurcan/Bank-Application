using bankApp.Business.Abstract;
using bankApp.Business.Concrete;
using bankApp.DTO;
using bankApp.Entity;
using bankApp.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


// AuthController sınıfının amacı, bir kullanıcı doğrulama (authentication) işlemi yapmak ve JWT (JSON Web Token) oluşturmak.
// AuthController sınıfı, kullanıcıların sisteme giriş yapmalarını sağlayan login endpoint'ini içerir. Kullanıcı giriş yaptıktan sonra, ona bir JWT token verilir.
// Bu token, kullanıcının sistemdeki kimliğini doğrulamak ve sonraki isteklerde kimlik doğrulaması yapmak için kullanılır.

namespace bankApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; // Kullanıcı yönetim işlevlerini yöneten servistir.

        private readonly IConfiguration _configuration; // JWT gizli anahtar burada saklanır.

        private readonly AccountService _accountService; //  Kullanıcı hesabı ile ilgili işlemleri yöneten servistir.

        public AuthController(IUserService userService, IConfiguration configuration, AccountService accountService)
        {
            _userService = userService;
            _configuration = configuration;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)

        // [FromBody] LoginDto loginDto -> Kullanıcıdan gelen JSON verisi LoginDto modeline dönüştürülür.
        // Bu model, kullanıcının TC kimlik numarası ve şifresini içerir.
        
        {
            if (loginDto.Tc.Length != 11)
                return BadRequest("TC Kimlik numarası 11 haneli olmalıdır.");

            var user = await _userService.LoginAsync(loginDto.Tc, loginDto.Password);
            if (user == null)
                return Unauthorized("TC kimlik numarası veya şifre hatalı.");

            // Hesabın aktif olup olmadığını kontrol ediyoruz
            bool isActive = await _accountService.IsAccountActiveAsync(user.UserId);
            if (!isActive)
            {
                return BadRequest(new { message = "Hesabınız kapalı. Lütfen destek ile iletişime geçin." });
            }

            var token = GenerateJwtToken(user); //  Kullanıcı için bir JWT token oluşturur.
            return Ok(new
            {
                token,
                user.UserId,
                user.Tc,
                user.Email
            
            }); // başarılı olursa token, userId, Tc ve email döndürülür.
            
        }

        // **JWT Token oluşturma fonksiyonu**
        private string GenerateJwtToken(User user)
        {
            var claims = new[] // Token içinde kullanıcının kimlik bilgileri yer alır (ID, isim, soyisim, e-posta).
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Surname, user.Surname),
        new Claim(ClaimTypes.Email, user.Email),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            // SymmetricSecurityKey -> JWT'nin güvenliğini sağlamak için kullanılan gizli anahtar'ı temsil eder.
            // appsettings.json dosyasındaki Jwt:Secret ayarı

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // SigningCredentials -> JWT'nin imzasını oluşturmak için kullanılır.
            // HMAC (Hash-based Message Authentication Code), mesajın doğruluğunu sağlamak için kullanılan bir algoritmadır
            // SHA256 ise bu algoritmanın hash fonksiyonudur.


            var token = new JwtSecurityToken( // JwtSecurityToken -> Bu sınıf, token'ı oluşturur ve içindeki bilgileri belirler

                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            // JwtSecurityTokenHandler -> oluşturulan JWT token'ı bir string formatına dönüştürür ve döndürür.
        }
    }
}
