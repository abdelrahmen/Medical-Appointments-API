using Medical_Appointments_API.Data;
using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appointments_API.Repositories
{
	public class MedicalHistoryRepository : IMedicalHistoryRepository
	{
		private readonly AppDbContext context;

		public MedicalHistoryRepository(AppDbContext context)
		{
			this.context = context;
		}

		public async Task CreateAsync(MedicalHistory medicalHistory)
		{
			context.medicalHistories.Add(medicalHistory);
			await context.SaveChangesAsync();
		}

		public Task DeleteAsync(int medicalHistoryId)
		{
			throw new NotImplementedException();
		}

		//gets all histories, requires admin previleges
		public async Task<IEnumerable<MedicalHistory>> GetAllAsync(int pageNumber, int pageSize)
		{
			if (pageNumber <= 0 || pageSize <= 0)
			{
				throw new ArgumentException("Invalid page number or page size.");
			}

			int itemsToSkip = (pageNumber - 1) * pageSize;

			var medicalHistories = await context.medicalHistories
				.Skip(itemsToSkip)
				.Take(pageSize)
				.ToListAsync();

			return medicalHistories;
		}

		public async Task<MedicalHistory?> GetByIdAsync(int medicalHistoryId)
		{
			return await context.medicalHistories.FirstOrDefaultAsync(mh => mh.MedicalHistoryID == medicalHistoryId);
		}

		public async Task<List<MedicalHistory>?> GetByPatientIdAsync(string patientId)
		{
			return await context.medicalHistories.Where(mh=>mh.UserId == patientId).ToListAsync();
		}

		public async Task UpdateAsync(MedicalHistory medicalHistory)
		{
			context.medicalHistories.Update(medicalHistory);
			await context.SaveChangesAsync();
		}
	}
}
