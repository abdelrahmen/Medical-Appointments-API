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

		// GET: api/MedicalHistory
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

		[HttpGet("patient/{patientId}")]
		[Authorize]
		public async Task<IActionResult> GetAllMedicalHistoryByPatientId(string patientId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			try
			{
				var medicalHistory = await medicalHistoryRepository.GetByPatientIdAsync(patientId);
				if (medicalHistory == null)
				{
					return NotFound();
				}

				if (!User.IsInRole("MedicalProfessional") && !User.IsInRole("Admin"))
				{
					var isAuthorized = medicalHistory.TrueForAll(a => a.UserId.Equals(userId));
					if (!isAuthorized)
					{
						return Unauthorized();
					}
				}

				return Ok(medicalHistory);

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

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

					return CreatedAtAction(nameof(GetMedicalHistory), new { id = medicalHistory.MedicalHistoryID }, medicalHistory);
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}
			return BadRequest(ModelState);
		}

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
