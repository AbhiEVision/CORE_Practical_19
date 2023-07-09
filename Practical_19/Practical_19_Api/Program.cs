using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practical_19_Api.Data;
using Practical_19_Api.Interfaces.Repository;
using Practical_19_Api.Interfaces.Services;
using Practical_19_Api.Repository;
using Practical_19_Api.Services;

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



			builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 5;
			}).AddEntityFrameworkStores<AppDbContext>();

			builder.Services.AddScoped<IAccessRepository, AccessRepository>();

			builder.Services.AddScoped<IUserServices, UserServices>();



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