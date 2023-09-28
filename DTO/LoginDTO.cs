using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }
		[Required]
		public string Password { get; set; }
    }
}