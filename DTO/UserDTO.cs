using Medical_Appointments_API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Medical_Appointments_API.DTO
{
    public class UserDTO
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }



        public static UserDTO FromApplicationUser(ApplicationUser user)
        {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
            };
        }
    }
}
