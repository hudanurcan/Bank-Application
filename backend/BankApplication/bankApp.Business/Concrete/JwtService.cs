using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


// Bu sınıfın amacı, bir JWT kullanarak, token'dan kullanıcı bilgilerini çıkartmaktır.
// JWT, genellikle kullanıcıların kimliğini doğrulamak ve yetkilendirme sağlamak için kullanılır.
// Bu sınıf ise token'ı alır ve bu token'dan kullanıcıya ait ID'yi elde etmeye yarar.

namespace bankApp.Business.Concrete
{
    public class JwtService
    {
        private readonly string _secretKey = "your-secret-key"; 

        public int GetUserIdFromToken(string token) // Bu metodun amacı, JWT'den kullanıcı ID'sini çıkartmaktır.
                                                    // string token parametresi -> JWT token'ını temsil eder. 
        {
            if (string.IsNullOrEmpty(token))
                return 0;

            var handler = new JwtSecurityTokenHandler(); //JwtSecurityTokenHandler sınıfı örnekleniyor.
                                                         //Bu sınıf, JWT token'larını okuma, oluşturma ve doğrulama gibi işlemleri yapar.
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            // ReadToken(token) metodu -> verilen token'ı alır ve onu bir JwtSecurityToken nesnesine dönüştürür.
            // as JwtSecurityToken kullanılarak, token’ın geçerli bir JWT token olup olmadığı kontrol edilir.
            // Eğer geçerli bir token ise, jsonToken değişkeni bir JwtSecurityToken nesnesine dönüşür.



            // JWT token’ları, claim adı verilen bilgileri içerir.
            // Claim’ler, token içerisinde saklanan, genellikle kullanıcıyla ilgili bilgileri (ID, ad, rol vs.) temsil eder.

            var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            // Bu LINQ sorgusu, Claims koleksiyonunda, Type özelliği NameIdentifier olan ilk öğeyi arar.
            // NameIdentifier, genellikle kullanıcının ID'sini tutan claim türüdür.

            // jsonToken?.Claims ifadesi, token'dan claim bilgilerini alır.
            // ?. operatörü, null kontrolü yapar, yani jsonToken null ise, claim'lere erişim sağlanmaz.

            if (userIdClaim != null)
                return int.Parse(userIdClaim.Value); 

            return 0; // UserId bulunamazsa
        }
    }
}
