using Medical_Appointments_API.Data.Models;

namespace Medical_Appointments_API.Repositories.Interfaces
{
	public interface IMedicalHistoryRepository
	{
		Task<IEnumerable<MedicalHistory>> GetAllAsync(int pageNumber, int pageSize);
		Task<MedicalHistory?> GetByIdAsync(int medicalHistoryId);
		Task<List<MedicalHistory>?> GetByPatientIdAsync(string patientId);
		Task CreateAsync(MedicalHistory medicalHistory);
		Task UpdateAsync(MedicalHistory medicalHistory);
		Task DeleteAsync(int medicalHistoryId);
	}
}
