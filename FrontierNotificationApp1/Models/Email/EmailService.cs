using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontierNotificationApp1.Models.Email
{
    public class EmailService : IEmailService
    {
		private readonly IEmailConfiguration _emailConfiguration;

		public EmailService(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}


		public void Send(EmailMessage emailMessage)
		{
			try
			{
				var message = new MimeMessage();
				message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
				message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
				message.Subject = emailMessage.Subject;

				//You can specifiy how you want to send message body content in the parameter
				//such as in HTML, plain text, etc. 
				message.Body = new TextPart(TextFormat.Html)
				{
					Text = emailMessage.Content
				};

				//Be careful that the SmtpClient class is the latest version from .NET Mailkit not the framework!
				//use import: using MailKit.Net.Smtp
				using (var emailClient = new SmtpClient())
				{
					//The last parameter here is to use SSL (Which you should!)
					emailClient.Connect(_emailConfiguration.SmtpDomain, _emailConfiguration.SmtpPort, true);

					//Remove any OAuth functionality we won't be using it. 
					emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

					emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

					emailClient.Send(message);

					emailClient.Disconnect(true);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}
	}

	public interface IEmailService
	{
		void Send(EmailMessage emailMessage);

	}
}
