using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
		[MaxLength(50)]
		public string FirstName { get; set; }
		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }
		[Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


	}
}