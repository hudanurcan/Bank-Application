using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.DTO
{
    public class RegisterDto
    {
        public int UserId { get; set; }  
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Düz şifre olarak alır.
        public string Tc { get; set; }  
        public string Phone { get; set; }  
        public string Address { get; set; }  
        public DateTime Birthday { get; set; }  
    }
}

// DTO (Data Transfer Object), istemciden (frontend) gelen verileri backend'e taşımak için kullanılan bir nesnedir.
// Yani, bir tür veri taşıyıcısıdır. DTO'lar, yalnızca veriyi taşımakla sorumludur, başka herhangi bir işlem yapmazlar.

// Bu sınıfta, Kullanıcıdan gelen bilgileri alıyoruz, ancak bu verileri doğrudan veritabanına kaydetmiyoruz.
// Bu verileri önce DTO sınıfında toplarız, sonra bu sınıfı backend'deki User sınıfına aktarırız.

// Yani frontend'den gelen bu bilgileri backend'de bu DTO üzerinden alıyoruz.