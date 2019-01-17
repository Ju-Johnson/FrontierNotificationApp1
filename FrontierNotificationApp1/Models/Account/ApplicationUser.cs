using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontierNotificationApp1.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
		public string UserId { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Department { get; set; }
	}
}
