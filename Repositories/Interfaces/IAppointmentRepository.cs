using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.DTO;

namespace Medical_Appointments_API.Repositories.Interfaces
{
	public interface IAppointmentRepository
	{
		Task<IEnumerable<Appointment>> GetAllAsync();
		Task<IEnumerable<Appointment>> GetAvailableAsync();//status = available
		Task<IEnumerable<Appointment>> GetAvailableBySpecialityAsync(string speciality);
		Task<IEnumerable<Appointment>> GetScheduledByPatientIdAsync(string patientId);
		Task<Appointment?> GetByIdAsync(int appointmentId, string userId);
		Task AddAsync(Appointment appointment);
		Task UpdateAsync(Appointment appointment);
		Task BookAsync(BookAppointmentDTO appointment);
		Task DeleteAsync(int appointmentId, string userId);

	}
}
