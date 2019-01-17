using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontierNotificationApp1.Models;
using FrontierNotificationApp1.Models.Email;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using FrontierNotificationApp1.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace FrontierNotificationApp1.Controllers
{
	
	public class IncidentsController : Controller
    {
        private readonly FrontierNotificationDBContext _context;
		private readonly IEmailService _emailService;
		private UserManager<ApplicationUser> _userManager;
		private string TeamList = "";

		//Dependency Injection of the SQL Server Database service provided in Startup.cs
		//Dependency Injection of Email Mailkit service provided in Startup.cs
		public IncidentsController(FrontierNotificationDBContext context, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_emailService = emailService;
			_userManager = userManager;
		}

		// GET: Incidents
		public async Task<IActionResult> Index(string searchString, int? page)
        {
			ViewData["currentFilter"] = searchString;
			

			//Setting the incident displayed sort order
			//most recent shown first on top
			var incidentsOrder = from i in _context.Incidents select i;

			if (!String.IsNullOrEmpty(searchString))
			{
				incidentsOrder = incidentsOrder.Where(i => i.Sdpticket.Contains(searchString));
			}
			else
			{
				incidentsOrder = incidentsOrder.OrderByDescending(i => i.Id);
			}

			int pageSize = 8;
			return View(await PaginatedList<Incidents>.CreateAsync(incidentsOrder.AsNoTracking(), page ?? 1, pageSize));
        }

		// GET: Incidents/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidents = await _context.Incidents
                .SingleOrDefaultAsync(m => m.Id == id);
            if (incidents == null)
            {
                return NotFound();
            }

            return View(incidents);
        }

		// GET: Incidents/Create
		[Authorize]
		public IActionResult Create()
        {
			List<ITteams> teams = new List<ITteams>();
			teams = (from i in _context.ITteams select i).ToList();
			ViewBag.TeamList = new MultiSelectList(teams, "name", "name", new[]{"Service Desk"});

			List<ITcontacts> contacts = new List<ITcontacts>();
			contacts = (from i in _context.ITcontacts select i).ToList();
			ViewBag.ContactList = new MultiSelectList(contacts, "email", "name");

			return View();
        }

        // POST: Incidents/Create
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] Teams, string[] Contacts, [Bind("Id,Priority,Sdpticket,Status,CurrentDateTime,Subject,Summary,Description,InitialDateTime,Impact,VendorTicket,TeamsEngaged")] Incidents incidents)
        {
			foreach (var i in Teams)
			{
				TeamList += i + ", ";
			}

			incidents.TeamsEngaged = TeamList;

			if (ModelState.IsValid)
            {
				
                _context.Add(incidents);
                await _context.SaveChangesAsync();

				var user = await _userManager.GetUserAsync(User);
				EmailAddress fromAddress = new EmailAddress();
				EmailMessage emailMessage = new EmailMessage();
				StringBuilder msgBody = new StringBuilder();

				var statusColor = "";
				switch (incidents.Status)
				{
					case "Open":
						statusColor = "\"color: white; background-color: red\""; //RED
						break;
					case "Monitoring":
						statusColor = "\"color: black; background-color: gold\""; //YELLOW
						break;
					case "Resolved":
						statusColor = "\"color: white; background-color: green\""; //GREEN
						break; 
				}

				msgBody.Append("<p><div><p style =" + statusColor + " align=\"center\">");
				msgBody.Append("<font size=5><b>" + incidents.Priority + " - Incident Notification - " + incidents.Status + "</b></font></p></div><br/>");
				msgBody.Append("<font size=3><div><b> SDP Ticket #: </b>" + incidents.Sdpticket + "</div><br/>");
				msgBody.Append("<div><b>Summary: </b>" + incidents.Summary + "</div><br/>");
				msgBody.Append("<div><b>Description: </b>" + incidents.Description + "</div><br/>");
				msgBody.Append("<div><b>Initial Date/Time: </b>" + incidents.InitialDateTime + "</div><br/>");
				msgBody.Append("<div><b>Impact: </b>" + incidents.Impact + "</div><br/>");
				msgBody.Append("<div><b>Vendor Ticket: </b>" + incidents.VendorTicket + "</div><br/>");
				msgBody.Append("<div><b>Teams Engaged: </b>" + TeamList + "</div><br/>");
				msgBody.Append("<div><b>Last Update Time: </b>" + incidents.CurrentDateTime + "</div></font></p>");

				var userName = user.FirstName + " " + user.LastName;
				var userEmail = user.Email;
				fromAddress.Name = userName;
				fromAddress.Address = userEmail;
				emailMessage.FromAddresses.Add(fromAddress);
				foreach (var i in Contacts)
				{
					EmailAddress toAddress = new EmailAddress();
					toAddress.Name = "";
					toAddress.Address = i;
					emailMessage.ToAddresses.Add(toAddress);
				}
				emailMessage.Subject = "" + incidents.Priority + " - Incident Notification - " + incidents.Status;
				emailMessage.Content = msgBody.ToString();
				_emailService.Send(emailMessage);

				return RedirectToAction(nameof(Index));
            }
            return View(incidents);
        }

		// GET: Incidents/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidents = await _context.Incidents.SingleOrDefaultAsync(m => m.Id == id);
            if (incidents == null)
            {
                return NotFound();
            }

			List<ITteams> teams = new List<ITteams>();
			teams = (from i in _context.ITteams select i).ToList();
			ViewBag.TeamList = new MultiSelectList(teams, "name","name", new[] { "Service Desk" });

			List<ITcontacts> contacts = new List<ITcontacts>();
			contacts = (from i in _context.ITcontacts select i).ToList();
			ViewBag.ContactList = new MultiSelectList(contacts, "email", "name");

			return View(incidents);
        }

        // POST: Incidents/Edit/5
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] Teams, string[] Contacts, [Bind("Id,Priority,Sdpticket,Status,CurrentDateTime,Subject,Summary,Description,InitialDateTime,Impact,VendorTicket,TeamsEngaged")] Incidents incidents)
        {

			if (id != incidents.Id)
            {
                return NotFound();
            }

			foreach(var i in Teams)
			{
				TeamList += i + ", ";
			}

			incidents.TeamsEngaged = TeamList;

			if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(incidents);
                    await _context.SaveChangesAsync();

					var user = await _userManager.GetUserAsync(User);
					EmailAddress fromAddress = new EmailAddress();
					EmailMessage emailMessage = new EmailMessage();
					StringBuilder msgBody = new StringBuilder();

					var statusColor = "";
					switch (incidents.Status)
					{
						case "Open":
							statusColor = "\"color: white; background-color: red\""; //RED
							break;
						case "Monitoring":
							statusColor = "\"color: black; background-color: gold\""; //YELLOW
							break;
						case "Resolved":
							statusColor = "\"color: white; background-color: green\""; //GREEN
							break;
					}

					msgBody.Append("<p><div><p style ="+statusColor+" align=\"center\">");
					msgBody.Append("<font size=5><b>" + incidents.Priority + " - Incident Notification - " + incidents.Status + "</b></font></p></div><br/>");
					msgBody.Append("<font size=3><div><b> SDP Ticket #: </b>" + incidents.Sdpticket + "</div><br/>");
					msgBody.Append("<div><b>Summary: </b>" + incidents.Summary + "</div><br/>");
					msgBody.Append("<div><b>Description: </b>" + incidents.Description + "</div><br/>");
					msgBody.Append("<div><b>Initial Date/Time: </b>" + incidents.InitialDateTime + "</div><br/>");
					msgBody.Append("<div><b>Impact: </b>" + incidents.Impact + "</div><br/>");
					msgBody.Append("<div><b>Vendor Ticket: </b>" + incidents.VendorTicket + "</div><br/>");
					msgBody.Append("<div><b>Teams Engaged: </b>" + TeamList + "</div><br/>");
					msgBody.Append("<div><b>Last Update Time: </b>" + incidents.CurrentDateTime + "</div></font></p>");

					
					var userName = user.FirstName + " " + user.LastName;
					var userEmail = user.Email;
					fromAddress.Name = userName;
					fromAddress.Address = userEmail;
					emailMessage.FromAddresses.Add(fromAddress);
					foreach (var i in Contacts)
					{
						EmailAddress toAddress = new EmailAddress();
						toAddress.Name = "";
						toAddress.Address = i;
						emailMessage.ToAddresses.Add(toAddress);
					}
					emailMessage.Subject = "" + incidents.Priority + " - Incident Notification - " + incidents.Status;
					emailMessage.Content = msgBody.ToString();
					_emailService.Send(emailMessage);

				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentsExists(incidents.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(incidents);
        }

		// GET: Incidents/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidents = await _context.Incidents
                .SingleOrDefaultAsync(m => m.Id == id);
            if (incidents == null)
            {
                return NotFound();
            }

            return View(incidents);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidents = await _context.Incidents.SingleOrDefaultAsync(m => m.Id == id);
            _context.Incidents.Remove(incidents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentsExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
    }
}
