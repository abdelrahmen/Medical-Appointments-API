using Medical_Appointments_API.Data.Models;
using Medical_Appointments_API.DTO;
using Medical_Appointments_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medical_Appointments_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicalHistoryController : ControllerBase
	{
		private readonly IMedicalHistoryRepository medicalHistoryRepository;

		public MedicalHistoryController(IMedicalHistoryRepository medicalHistoryRepository)
		{
			this.medicalHistoryRepository = medicalHistoryRepository;
		}

		/// <summary>
		/// Retrieves a paginated list of all medical history records for administrators.
		/// </summary>
		/// <param name="pageNumber">The page number for pagination (default is 1).</param>
		/// <param name="pageSize">The number of records per page (default is 10).</param>
		/// <returns>
		/// - 200 OK with a paginated list of medical history records.
		/// - 401 Unauthorized if the user is not authenticated.
		/// - 403 Forbidden if the user does not have the 'Admin' role.
		/// - 500 Internal Server Error if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// GET /api/MedicalHistory/all?pageNumber=2&pageSize=20
		/// Authorization: Bearer [Your JWT Token]
		/// </example>
		[HttpGet("all")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllMedicalHistories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			try
			{
				var medicalHistories = await medicalHistoryRepository.GetAllAsync(pageNumber, pageSize);
				return Ok(medicalHistories);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Retrieves a specific medical history record by its unique identifier.
		/// </summary>
		/// <param name="historyId">The unique identifier of the medical history record to retrieve.</param>
		/// <returns>
		/// - 200 OK with the medical history record if found and user has appropriate access.
		/// - 401 Unauthorized if the user is not authorized to access the record.
		/// - 404 Not Found if the record with the specified identifier doesn't exist.
		/// - 400 Bad Request if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// GET /api/MedicalHistory/123
		/// Authorization: Bearer [Your JWT Token]
		/// </example>
		[HttpGet("{historyId}")]
		[Authorize]
		public async Task<IActionResult> GetMedicalHistory(int historyId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			try
			{
				var medicalHistory = await medicalHistoryRepository.GetByIdAsync(historyId);
				if (medicalHistory == null)
				{
					return NotFound();
				}

				if (medicalHistory.UserId != userId && !User.IsInRole("MedicalProfessional"))
				{
					return Unauthorized();
				}

				return Ok(medicalHistory);

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Retrieves all medical history records associated with a specific patient.
		/// </summary>
		/// <param name="patientId">The unique identifier of the patient whose medical history records are to be retrieved.</param>
		/// <returns>
		/// - 200 OK with the list of medical history records if found and user has appropriate access.
		/// - 401 Unauthorized if the user is not authorized to access the records.
		/// - 404 Not Found if no records are found for the specified patient or if the patient doesn't exist.
		/// - 400 Bad Request if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// GET /api/MedicalHistory/patient/123
		/// Authorization: Bearer [Your JWT Token]
		/// </example>
		[HttpGet()]
		[Authorize]
		public async Task<IActionResult> GetAllMedicalHistoryByPatientId()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			try
			{
				var medicalHistory = await medicalHistoryRepository.GetByPatientIdAsync(userId);
				if (medicalHistory == null)
				{
					return NotFound();
				}
				return Ok(medicalHistory);

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Creates a new medical history record for the authenticated user.
		/// </summary>
		/// <param name="medicalHistoryDTO">The data for creating the medical history record.</param>
		/// <returns>
		/// - 201 Created with the newly created medical history record if successful.
		/// - 400 Bad Request if the request data is invalid.
		/// - 401 Unauthorized if the user is not authenticated.
		/// - 500 Internal Server Error if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// POST /api/MedicalHistory
		/// Authorization: Bearer [Your JWT Token]
		/// Request Body:
		/// {
		///     "MedicalCondition": "Hypertension",
		///     "Medications": "Lisinopril, Amlodipine",
		///     "Allergies": "None",
		///     "Surgeries": "Appendectomy",
		///     "FamilyMedicalHistory": "Heart disease, diabetes"
		/// }
		/// </example>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateMedicalHistory(CreateMedicalHistoryDTO medicalHistoryDTO)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var medicalHistory = new MedicalHistory
					{
						UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
						MedicalCondition = medicalHistoryDTO.MedicalCondition,
						Medications = medicalHistoryDTO.Medications,
						Allergies = medicalHistoryDTO.Allergies,
						Surgeries = medicalHistoryDTO.Surgeries,
						FamilyMedicalHistory = medicalHistoryDTO.FamilyMedicalHistory,
					};
					await medicalHistoryRepository.CreateAsync(medicalHistory);

					return CreatedAtAction(nameof(GetMedicalHistory), new { historyId = medicalHistory.MedicalHistoryID }, medicalHistory);
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}
			return BadRequest(ModelState);
		}

		/// <summary>
		/// Updates an existing medical history record for the authenticated user.
		/// </summary>
		/// <param name="historyId">The ID of the medical history record to update.</param>
		/// <param name="medicalHistoryDTO">The data for updating the medical history record.</param>
		/// <returns>
		/// - 200 OK with the updated medical history record if successful.
		/// - 400 Bad Request if the request data is invalid.
		/// - 401 Unauthorized if the user is not authorized to update the record.
		/// - 404 Not Found if the requested medical history record does not exist.
		/// - 500 Internal Server Error if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// PUT /api/MedicalHistory/3
		/// Authorization: Bearer [Your JWT Token]
		/// Request Body:
		/// {
		///     "Medications": "Lisinopril, Amlodipine, Hydrochlorothiazide",
		///     "Allergies": "Pollen",
		///     "Surgeries": "Appendectomy, Tonsillectomy",
		///     "FamilyMedicalHistory": "Heart disease, diabetes, cancer"
		/// }
		/// </example>
		[HttpPut("{historyId}")]
		[Authorize]
		public async Task<IActionResult> UpdateMedicalHistory(int historyId, UpdateMedicalHistoryDTO medicalHistoryDTO)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					var currentHistory = await medicalHistoryRepository.GetByIdAsync(historyId);

					if (currentHistory == null)
					{
						return NotFound();
					}

					if (!currentHistory.UserId.Equals(userId))
					{
						return Unauthorized();
					}

					currentHistory.Medications = medicalHistoryDTO.Medications;
					currentHistory.FamilyMedicalHistory = medicalHistoryDTO.FamilyMedicalHistory;
					currentHistory.Surgeries = medicalHistoryDTO.Surgeries;
					currentHistory.Allergies = medicalHistoryDTO.Allergies;
					await medicalHistoryRepository.UpdateAsync(currentHistory);
					return Ok(currentHistory);

				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}
			else
			{
				return BadRequest(ModelState);
			}
		}

		/// <summary>
		/// Deletes a specific medical history record for the authenticated user.
		/// </summary>
		/// <param name="historyId">The ID of the medical history record to delete.</param>
		/// <returns>
		/// - 204 No Content if the record is successfully deleted.
		/// - 401 Unauthorized if the user is not authorized to delete the record.
		/// - 404 Not Found if the requested medical history record does not exist.
		/// - 500 Internal Server Error if an error occurs during the operation.
		/// </returns>
		/// <example>
		/// Example Request:
		/// DELETE /api/MedicalHistory/3
		/// Authorization: Bearer [Your JWT Token]
		/// </example>
		[HttpDelete("{historyId}")]
		[Authorize]
		public async Task<IActionResult> DeleteMedicalHistory(int historyId)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var currentHistory = await medicalHistoryRepository.GetByIdAsync(historyId);

				if (currentHistory == null)
				{
					return NotFound();
				}

				if (!currentHistory.UserId.Equals(userId))
				{
					return Unauthorized();
				}

				await medicalHistoryRepository.DeleteAsync(historyId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
