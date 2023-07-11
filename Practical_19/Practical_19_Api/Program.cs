using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practical_19_Api.Data;
using Practical_19_Api.Interfaces.Repository;
using Practical_19_Api.Interfaces.Services;
using Practical_19_Api.Repository;
using Practical_19_Api.Services;
using System.Text;

namespace Practical_19_Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});




			builder.Services.AddAuthentication(auth =>
			{
				auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(option =>
			{
				option.TokenValidationParameters = new TokenValidationParameters()
				{
					RequireExpirationTime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthKey"])),
					ValidateIssuerSigningKey = true,
				};


				option.Events.OnMessageReceived = context =>
				{
					if (context.Request.Cookies.ContainsKey("AuthToken"))
					{
						context.Token = context.Request.Cookies["AuthToken"];
					}
					return Task.CompletedTask;
				};

			});
			builder.Services.AddAuthorization();


			builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 5;
			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();


			builder.Services.AddScoped<IAccessRepository, AccessRepository>();

			builder.Services.AddScoped<IUserServices, UserServices>();

			builder.Services.AddScoped<IUserRepository, UserRepository>();


			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();


			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				//app.UseSwagger();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();



			app.Run();
		}
	}
}