using Medical_Appointments_API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
    public class CreateAppointmentDTO
    {
		public string? PatientId { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }
	}
}