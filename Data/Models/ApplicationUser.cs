using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }
		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		public string? Specialty { get; set; } //for doctors

		public ICollection<Appointment> Appointments { get; set; }
		public ICollection<MedicalHistory> MedicalHistories { get; set; } //for patients
	}
}