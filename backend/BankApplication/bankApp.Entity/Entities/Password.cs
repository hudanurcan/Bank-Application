using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace bankApp.Entity.Entities
{
    //public class Password
    //{
    //    [Key] 
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // PasswordId'yi otomatik artırır. bu, pk'ye uygulanır.
    //    public int PasswordId { get; set; }
    //    public int UserId { get; set; }  // şifrenin hangi kullanıcıya ait olduğunu belirtir
    //    public string? PasswordHash { get; set; } // Hashlenmiş şifre

    //    [JsonIgnore] // [JsonIgnore] :  Bir özelliğin JSON'a dahil edilmemesini sağlar. 
    //    public virtual User? User { get; set; }  // Kullanıcıyı referans alır. Bu alan JSON'a dahil edilmez
    //}
    public class Password
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PasswordId { get; set; } 

        public string PasswordHash { get; set; }

        public int UserId { get; set; }

        [JsonIgnore] // [JsonIgnore] :  Bir özelliğin JSON'a dahil edilmemesini sağlar. 
        public virtual User User { get; set; }
    }


}
