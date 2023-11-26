using Medical_Appointments_API.Data;
using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.Repositories;
using Medical_Appointments_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Medical_Appointments_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Configuration.AddJsonFile("appsettings.json");

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<AppDbContext>();
			builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
			builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
				options =>
				{
					options.User.RequireUniqueEmail = true;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
				}).AddEntityFrameworkStores<AppDbContext>();

			var config = builder.Configuration;

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				//options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = config["Jwt:Issuer"],
					ValidAudience = config["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
				};
			}); ;

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors(config => {
				//config.WithOrigins("http://localhost:4200");
				config.AllowAnyOrigin();
				config.AllowAnyMethod();
				config.AllowAnyHeader();
			});

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}