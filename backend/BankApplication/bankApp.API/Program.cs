using bankApp.Business.Abstract;
using bankApp.DataAccess.Abstract;
using bankApp.DataAccess.Concrete;
using bankApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using bankApp.Business.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();  
                                       
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "",
                    ValidAudience = "",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyThatIsAtLeast32CharactersLong!"))
                };
            });

        
        builder.Services.AddAuthorization();
        
        builder.Services.AddDbContext<BankAppContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("bankApp.DataAccess"))); 

        builder.Services.AddScoped<IUserService, UserManager>(); 
        builder.Services.AddScoped<IUserDal, EfUserDal>(); 
        builder.Services.AddScoped<AccountService>();  
        builder.Services.AddScoped<TransactionService>();
        builder.Services.AddScoped<JwtService>();
        // CORS'u etkinleþtir
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
                builder.WithOrigins("http://localhost:4200")  
                       .AllowAnyMethod()  // Herhangi bir HTTP metoduna izin ver
                       .AllowAnyHeader()
                       .AllowCredentials()); // Herhangi bir baþlýða izin ver
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseRouting();  
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthentication();  // **Kimlik Doðrulama Middleware**
        app.UseAuthorization();   // **Yetkilendirme Middleware**
        // CORS Middleware'i burada çalýþtýrýlmalý
       // app.UseCors("AllowAllOrigins");

        app.UseSwagger();
        app.UseSwaggerUI();

    //    app.UseHttpsRedirection();  // HTTPS sorunu için geçici olarak kaldýrýldý. 
        app.MapControllers();
        app.Run();

    }
}