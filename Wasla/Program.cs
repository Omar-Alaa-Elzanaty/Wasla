using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Wasla.DataAccess;
using Wasla.Model.Models;
using Wasla.Services;
using Wasla.Services.Initizalize;

namespace Wasla
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddServices();
			builder.Services.AddDbContext<WaslaDb>(option =>
				option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
									b => b.MigrationsAssembly(typeof(WaslaDb).Assembly.FullName))
									);
			builder.Services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<WaslaDb>()
				.AddDefaultTokenProviders();
			
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = false;
				o.SaveToken = false;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidAudience = builder.Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
					ClockSkew = TimeSpan.Zero
				};
			});
			builder.Services.AddSwaggerGen();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddCors();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			DataSeed(app);

			app.UseStaticFiles();
			app.UseCors(cores =>
							cores.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
			app.UseRouting();

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
		static void  DataSeed(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var Initalizer = scope.ServiceProvider.GetRequiredService<IInitializer>();
			Initalizer.Initialize().Wait();
		}
	}
}