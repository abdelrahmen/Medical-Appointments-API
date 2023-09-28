using Medical_Appointments_API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Medical_Appointments_API.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{

		public DbSet<Appointment> appointments { get; set; }
		public DbSet<MedicalHistory> medicalHistories { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<IdentityRole>().HasData(SeedRoles());
			builder.Entity<ApplicationUser>().HasData(SeedSuperAdmin());
			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					RoleId = "1", // RoleId for SuperAdmin
					UserId = "1" // User Id for the default user
				}
			);

			builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			var connectionString = Configuration.GetConnectionString("local");
			optionsBuilder.UseSqlServer(connectionString);
		}

		private List<IdentityRole> SeedRoles()
		{
			return new List<IdentityRole>
			{
				new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
				new IdentityRole { Id = "2", Name = "MedicalProfessional", NormalizedName = "MEDICALPROFESSIONAL" },
				new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "PATIENT" }
			};
		}

		private ApplicationUser SeedSuperAdmin()
		{
			var hasher = new PasswordHasher<ApplicationUser>();
			return new ApplicationUser
			{
				Id = "1",
				UserName = "TemporaryUsername",
				NormalizedUserName = "TEMPORARY-USERNAME",
				Email = "TemporaryEmail@example.com",
				NormalizedEmail = "TEMPORARYEMAIL@EXAMPLE.COM",
				FirstName = "Temporary first Name",
				LastName = "Temporary last Name",
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "TemporaryPassword"),
				SecurityStamp = string.Empty
			};
		}
	}
}

