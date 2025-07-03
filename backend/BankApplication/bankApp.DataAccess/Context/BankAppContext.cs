using bankApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Veritabanı bağlantısı için BankAppContext sınıfı kullanılır.
// Bu sınıf, DbContext'ten türetilmiştir ve veritabanındaki kullanıcılar gibi tablolarla etkileşim sağlar.
// DbSet ile tablolara erişim sağlar

// BankAppContext sınıfı, Entity Framework (EF) kullanarak uygulamanın veritabanı işlemlerini yönetir.
// Bu sınıf, DbContext sınıfından türemektedir ve veritabanı tablosu ile uygulama arasında köprü vazifesi görür.
// Yani, veritabanı ile yapılacak işlemleri sağlar ve yönetir.

// Veritabanı ilişkilerini tanımlar. Tablo arasındaki ilişkileri  burada tanımlarsınız.

namespace bankApp.DataAccess.Context
{
    public class BankAppContext : DbContext
    {

        public BankAppContext(DbContextOptions<BankAppContext> options) : base(options)
        // base(options) ile DbContext sınıfının constructor'ı çağrılır ve veritabanı bağlantısı başlatılır.
        {
        }

        public DbSet<User> Users { get; set; } // Veritabanındaki Users tablosuna karşılık gelir.
                                               // DbSet<User>, User entity'sinin veritabanında nasıl saklandığını belirler.
        public DbSet<Password> Passwords { get; set; } 
        public DbSet<BankAccount> BankAccounts { get; set; } 

        public DbSet<Transaction> Transactions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder) // Veritabanı ilişkilerini ve tablo ayarlarını burada yapıyoruz.
       
        {
            //User ve Password sınıfları arasında bir bire bir ilişki 
            modelBuilder.Entity<User>()
                .HasOne(u => u.Password)
                .WithOne(p => p.User)
                .HasForeignKey<Password>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // bir User silindiğinde, ona bağlı Password da silinir

            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.User)  
                .WithMany(u => u.BankAccounts)  // Kullanıcı birden fazla hesaba sahip olabilir
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Kullanıcı silindiğinde hesaplar da silinir
                .OnDelete(DeleteBehavior.SetNull);  // Banka hesabı silindiğinde, UserId null olur.


            modelBuilder.Entity<BankAccount>()
                .Property(b => b.Balance)
                .HasColumnType("decimal(10,2)");  // Balance için decimal(10,2) formatı. toplam 10 basamak, virgülden sonra max 2 basamak 

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Transaction>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()"); // Veritabanı saati

            modelBuilder.Entity<Transaction>()
                .Property(t => t.DateOfTransaction)
                .HasDefaultValueSql("GETDATE()"); // Veritabanı saati
        }




    }
}


