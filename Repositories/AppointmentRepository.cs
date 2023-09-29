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

		public async Task<Appointment?> GetByIdAsync(int appointmentId, string userId)
		{
            var appointment = await context.appointments.FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
            if (appointment != null)
            {
                if (appointment.PatientId == userId || appointment.DoctorId ==userId)
                {
					return appointment;
                }
                else
                {
                    throw new Exception("You are not allowed to view this appointment");
				}
            }
			return null;
        }

		public async Task<IEnumerable<Appointment>> GetAllAsync()
		{
			return await context.appointments.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAvailableAsync()
		{
			var available = await context.appointments.Where(a => a.Status.Equals("Available")).ToListAsync();
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

		public async Task<IEnumerable<Appointment>> GetScheduledByPatientIdAsync(string patientId)
		{
			var appointments = await context.appointments.Where(a => a.PatientId == patientId).ToListAsync();
			return appointments;
		}
	}
}
