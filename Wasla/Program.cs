using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using Wasla.DataAccess;
using Wasla.DataAccess.AutoMapping;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.ApplicationStatic;
using Wasla.Services.AuthService;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.Initizalize;
using Wasla.Services.MultLanguageService.JsonLocalizer;

namespace Wasla
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();            
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                  new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
                }
            });
            });

            
            // Add services to the container.
           builder.Services.AddServices();
			builder.Services.AddDbContext<WaslaDb>(option =>
				option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
									b => b.MigrationsAssembly(typeof(WaslaDb).Assembly.FullName))
									);
			builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<WaslaDb>().AddDefaultTokenProviders();
            builder.Services.AddDistributedMemoryCache();
           
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;//true
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,//false
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddControllers();
            builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddAutoMapper(typeof(AuthAutoMapper));
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IInitializer, Initializer>();

            //
            builder.Services.AddScoped<ValidationFilterAttribute>();
            builder.Services.Configure<ApiBehaviorOptions>(options
           => options.SuppressModelStateInvalidFilter = true);
            
            //
            //loclazation

            builder.Services.AddLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                  new CultureInfo("en-US"),
                  new CultureInfo("ar-EG"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
                options.SupportedCultures = supportedCultures;
            });
            builder.Services.AddMvc()
              .AddDataAnnotationsLocalization(options =>
              {
               options.DataAnnotationLocalizerProvider = (type, factory) =>
               factory.Create(typeof(JsonStringLocalizerFactory));
              });
            //end loclatzation
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddCors();
            builder.Services.AddAuthorization();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            //

            //
        
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            DataSeed(app);
            //
            var supportedCultures = new[] { "en-US", "ar-EG" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
            //
            app.AddGlobalExceptionGlobalHandler();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(cores => cores.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
			app.MapControllers();
            app.Run();

        }
        static void  DataSeed(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var Initalizer = scope.ServiceProvider.GetRequiredService<IInitializer>();
			//Initalizer.Initialize().Wait();
		}
        
    }
}