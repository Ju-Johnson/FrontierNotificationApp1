using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontierNotificationApp1.Models.Email
{
    public class EmailConfiguration : IEmailConfiguration
    {
		//Set in the appsettings.json file
		//Then loaded in the startup.cs file
		public string SmtpDomain { get; set; } 
		public int SmtpPort { get; set; }      
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }

		public string PopServer { get; set; }
		public int PopPort { get; set; }
		public string PopUsername { get; set; }
		public string PopPassword { get; set; }
	}

	public interface IEmailConfiguration
	{
		string SmtpDomain { get; }
		int SmtpPort { get; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }

		string PopServer { get; }
		int PopPort { get; }
		string PopUsername { get; }
		string PopPassword { get; }
	}
}
