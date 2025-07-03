//using Microsoft.EntityFrameworkCore.Design;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

//namespace bankApp.DataAccess.Context
//{
//    public class DataContextFactory : IDesignTimeDbContextFactory<BankAppContext>
//    {
//        public BankAppContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<BankAppContext>();
//            optionsBuilder.UseSqlServer("Server=DESKTOP-J5LIJ2T\\SQLEXPRESS;Database=BankApplication;Trusted_Connection=True;TrustServerCertificate=True;");  // Veritabanı bağlantınızı burada yapılandırın.

//            return new BankAppContext(optionsBuilder.Options);
//        }
//    }
//}
