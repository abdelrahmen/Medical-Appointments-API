using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
	public class BookAppointmentDTO
	{
		[Required]
		public int AppointmentID { get; set; }

		[Required]
		public string? PatientId { get; set; }

		[MaxLength(250)]
		public string Notes { get; set; }
	}
}
