using Medical_Appointments_API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
	public class AppointmentDTO
	{
		public int AppointmentID { get; set; }

		[Required]
		public string DoctorId { get; set; }

		public string? PatientId { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }

		[Required]
		[MaxLength(20)]
		public string Status { get; set; } // e.g., Available, scheduled, canceled, completed

		[MaxLength(250)]
		public string Notes { get; set; }

		public static AppointmentDTO FromAppointment(Appointment appointment)
		{
			return new AppointmentDTO
			{
				AppointmentID = appointment.AppointmentID,
				AppointmentDateTime = appointment.AppointmentDateTime,
				Status = appointment.Status,
				Notes = appointment.Notes,
				DoctorId = appointment.DoctorId,
				PatientId = appointment.PatientId,
			};
		}
	}
}