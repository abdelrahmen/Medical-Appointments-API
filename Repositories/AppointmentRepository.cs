using Medical_Appointments_API.Data;
using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.DTO;
using Medical_Appointments_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appointments_API.Repositories
{
	public class AppointmentRepository : IAppointmentRepository
	{
		private readonly AppDbContext context;

		public AppointmentRepository(AppDbContext context)
		{
			this.context = context;
		}

		public async Task AddAsync(Appointment appointment)
		{
			await context.appointments.AddAsync(appointment);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int appointmentId, string userId)
		{
			var appointment = await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
			if (appointment != null)
			{
				if (appointment.PatientId == userId || appointment.DoctorId == userId)
				{
					context.appointments.Remove(appointment);
					await context.SaveChangesAsync();
				}
				else
				{
					throw new Exception("You are not allowed to delete this appointment");
				}
			}
			else
			{
				throw new Exception("Appointment not found");
			}
		}

		public async Task<Appointment?> GetByIdAsync(int appointmentId)
		{
			return await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
		}

		public async Task<IEnumerable<Appointment>> GetAvailableBySpecialityAsync(string speciality, int pageNumber, int pageSize)
		{
			var appointmets = context.appointments
				.Include(a => a.Doctor)
				.Where(a => a.Status.Equals("Available") && a.AppointmentDateTime > DateTime.Now)
				.Where(a => a.Doctor.Specialty.Equals(speciality, StringComparison.OrdinalIgnoreCase));

			return await appointmets
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize).ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAllAsync(int pageNumber, int pageSize)
		{
			return await context.appointments
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAvailableAsync(int pageNumber, int pageSize)
		{
			var available = await context.appointments.Include(a => a.Doctor)
				.Where(a => a.Status.Equals("Available") && a.AppointmentDateTime > DateTime.Now)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize).ToListAsync();
			return available;
		}

		public async Task UpdateAsync(Appointment appointment)
		{
			var currentAppointment = await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointment.AppointmentID);
			if (currentAppointment != null)
			{
				context.appointments.Update(currentAppointment);
				context.SaveChanges();
			}
		}

		public async Task BookAsync(BookAppointmentDTO appointment)
		{
			var currentAppointment = await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointment.AppointmentID);
			if (currentAppointment != null)
			{
				currentAppointment.Status = "Scheduled";
				currentAppointment.PatientId = appointment.PatientId;
				currentAppointment.Notes = appointment.Notes;

				context.appointments.Update(currentAppointment);
			}
		}

		public async Task CancelAsync(int appointmentId, string userId)
		{
			var currentAppointment = await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
			if (currentAppointment != null)
			{
				if (currentAppointment.PatientId == userId || currentAppointment.DoctorId == userId)
				{
					currentAppointment.Status = "Canceled";
					currentAppointment.Notes += $"\n Appointment canceled by user: {userId}";
					await context.SaveChangesAsync();
				}
				else
				{
					throw new UnauthorizedAccessException("You are not authorized to cancel this appointment.");
				}
			}
			else
			{
				throw new ArgumentException("Appointment not found.");
			}
		}

		public async Task<IEnumerable<Appointment>> GetScheduledByPatientIdAsync(string patientId, int pageNumber, int pageSize)
		{
			var appointments = await context.appointments
				.Where(a => a.PatientId == patientId)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize).ToListAsync();
			return appointments;
		}
	}
}
