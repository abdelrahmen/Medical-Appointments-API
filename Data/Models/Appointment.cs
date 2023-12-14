using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.Data.Models
{
	//an appointmet is created mainly by a doctor
	//initialy with no patient and status = available
	//patients get the available appointments and can book them
	public class Appointment
	{
		[Key]
		public int AppointmentID { get; set; }

		[Required]
		public string DoctorId { get; set; }

		public string? PatientId { get; set; }

		[ForeignKey("DoctorId")]
		public ApplicationUser Doctor { get; set; }

		[ForeignKey("PatientId")]
		public ApplicationUser? Patient { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }

		[Required]
		[MaxLength(20)]
		public string Status { get; set; } // e.g., Available, scheduled, canceled, completed

		[MaxLength(250)]
		public string? Notes { get; set; }
	}
}
