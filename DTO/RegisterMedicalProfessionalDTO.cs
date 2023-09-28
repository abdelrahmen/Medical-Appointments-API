using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
    public class RegisterMedicalProfessionalDTO
	{
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
		public string Specialty { get; set; }


	}
}