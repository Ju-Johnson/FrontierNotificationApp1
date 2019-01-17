using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontierNotificationApp1.Models.Account
{
    public class Register
    {
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required, EmailAddress, MaxLength(256)]
		public string Email { get; set; }

		[MaxLength(255)]
		public string Department { get; set; }

		[Required, DataType(DataType.Password), MinLength(6), MaxLength(50)]
		public string Password { get; set; }

		[Required, DataType(DataType.Password), MinLength(6), MaxLength(50)]
		[Compare("Password", ErrorMessage ="Passwords do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
