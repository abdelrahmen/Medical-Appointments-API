using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.DTO;
using Medical_Appointments_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medical_Appointments_API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AppointmentsController : ControllerBase
	{
		private readonly IAppointmentRepository appointmentRepository;

		public AppointmentsController(IAppointmentRepository appointmentRepository)
		{
			this.appointmentRepository = appointmentRepository;
		}

		[HttpGet]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> GetAllAppointments()
		{
			var appointments = await appointmentRepository.GetAllAsync();
			return Ok(appointments);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetAppointment(int id)
		{
			var appointment = await appointmentRepository.GetByIdAsync(id);

			if (appointment == null)
			{
				return NotFound();
			}

			return Ok(appointment);
		}

		[HttpGet("Available")]
		public async Task<IActionResult> GetAvailableAppointments()
		{
			var appointments = await appointmentRepository.GetAvailableAsync();
			return Ok(appointments);
		}

		[HttpGet("my-appointments")]
		[Authorize]
		public async Task<IActionResult> GetScheduledAppointmentsByPatient() 
		{
			string patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var appointments = await appointmentRepository.GetScheduledByPatientIdAsync(patientId);
			return Ok(appointments);
		}

		[HttpPost]
		[Authorize(Roles = "MedicalProfessional")]
		public async Task<IActionResult> CreateAppointment(CreateAppointmentDTO appointmentDTO)
		{
			if (ModelState.IsValid)
			{
				var appointment = new Appointment
				{
					AppointmentDateTime = appointmentDTO.AppointmentDateTime,
					DoctorId = appointmentDTO.DoctorId,
					PatientId = appointmentDTO.PatientId,
				};
				await appointmentRepository.AddAsync(appointment);

				return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentID }, appointment);
			}
			return BadRequest(ModelState);
		}

		[HttpPut("Book/{id}")]
		[Authorize]
		public async Task<IActionResult> BookAppointment(int id, BookAppointmentDTO appointment)
		{
			if (id != appointment.AppointmentID)
			{
				return BadRequest("ID mismatch");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				await appointmentRepository.BookAsync(appointment);
				return Ok("appointment Booked Successfully");
			}
			catch (Exception ex)
			{
				return BadRequest("something wrong happend, please try again later");
			}
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteAppointment(int id)
		{
			await appointmentRepository.DeleteAsync(id);
			return NoContent();
		}
	}
}
