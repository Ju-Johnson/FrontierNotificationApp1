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
	
	public class ChangeResquestsController : Controller
    {
		private readonly FrontierNotificationDBContext _context;
		private readonly IEmailService _emailService;
		private UserManager<ApplicationUser> _userManager;
		private string TeamList = "";
		

		//Dependency Injection of the SQL Server Database service provided in Startup.cs
		//Dependency Injection of Email Mailkit service provided in Startup.cs
		public ChangeResquestsController(FrontierNotificationDBContext context, IEmailService emailService, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_emailService = emailService;
			_userManager = userManager;
		}

		// GET: ChangeResquests
		public async Task<IActionResult> Index(string searchString, int? page)
		{
			ViewData["currentFilter"] = searchString;

			//Setting the requests displayed sort order
			//most recent shown first on top
			var requestsOrder = from i in _context.ChangeResquests select i;

			if (!String.IsNullOrEmpty(searchString))
			{
				requestsOrder = requestsOrder.Where(i => i.Number.Contains(searchString));
			}
			else
			{
				requestsOrder = requestsOrder.OrderByDescending(i => i.Id);
			}

			int pageSize = 8;
			return View(await PaginatedList<ChangeResquests>.CreateAsync(requestsOrder.AsNoTracking(), page ?? 1, pageSize));
		}

		// GET: ChangeResquests/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var changeResquests = await _context.ChangeResquests
                .SingleOrDefaultAsync(m => m.Id == id);
            if (changeResquests == null)
            {
                return NotFound();
            }

            return View(changeResquests);
        }

		// GET: ChangeResquests/Create
		[Authorize]
		public IActionResult Create()
        {
			List<ITteams> teams = new List<ITteams>();
			teams = (from i in _context.ITteams select i).ToList();
			ViewBag.TeamList = new MultiSelectList(teams, "name", "name", new[] { "Service Desk" });

			List<ITcontacts> contacts = new List<ITcontacts>();
			contacts = (from i in _context.ITcontacts select i).ToList();
			ViewBag.ContactList = new MultiSelectList(contacts, "email", "name");

			return View();
        }

        // POST: ChangeResquests/Create
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] Teams, string[] Contacts, [Bind("Id,Number,Subject,Summary,Status,StartDateTime,EndDateTime,Impact,TeamsEngaged")] ChangeResquests changeResquests)
        {
			foreach (var i in Teams)
			{
				TeamList += i + ", ";
			}

			changeResquests.TeamsEngaged = TeamList;

			if (ModelState.IsValid)
            {
                _context.Add(changeResquests);
                await _context.SaveChangesAsync();

				var user = await _userManager.GetUserAsync(User);
				EmailAddress fromAddress = new EmailAddress();
				EmailMessage emailMessage = new EmailMessage();
				StringBuilder msgBody = new StringBuilder();

				msgBody.Append("<p><div><p style=\"background-color: #ADD8E6\" align=\"center\">");
				msgBody.Append("<font size=5><b> " + changeResquests.Status + " - Planned Maintenance - CR#: " + changeResquests.Number + "</b></font></p></div><br/>");
				msgBody.Append("<font size=3><div><b>Subject:</b> " + changeResquests.Subject + "</div><br/>");
				msgBody.Append("<div><b>Summary: </b>" + changeResquests.Summary + "</div><br/>");
				msgBody.Append("<div><b>Start Date/Time: </b>" + changeResquests.StartDateTime + "</div><br/>");
				msgBody.Append("<div><b>Impact: </b>" + changeResquests.Impact + "</div><br/>");
				msgBody.Append("<div><b>Teams Engaged: </b>" + TeamList + "</div><br/>");
				msgBody.Append("<div><b>Endt Date/Time: </b>" + changeResquests.EndDateTime + "</div></font></p>");

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
				emailMessage.Subject = "" + changeResquests.Status + " - Planned Maintenance - CR#: " + changeResquests.Number;
				emailMessage.Content = msgBody.ToString();
				_emailService.Send(emailMessage);

				return RedirectToAction(nameof(Index));
            }
            return View(changeResquests);
        }

		// GET: ChangeResquests/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var changeResquests = await _context.ChangeResquests.SingleOrDefaultAsync(m => m.Id == id);
            if (changeResquests == null)
            {
                return NotFound();
            }

			List<ITteams> teams = new List<ITteams>();
			teams = (from i in _context.ITteams select i).ToList();
			ViewBag.TeamList = new MultiSelectList(teams, "name", "name", new[] { "Service Desk" });

			List<ITcontacts> contacts = new List<ITcontacts>();
			contacts = (from i in _context.ITcontacts select i).ToList();
			ViewBag.ContactList = new MultiSelectList(contacts, "email", "name");

			return View(changeResquests);
        }

        // POST: ChangeResquests/Edit/5
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] Teams, string[] Contacts, [Bind("Id,Number,Subject,Summary,Status,StartDateTime,EndDateTime,Impact,TeamsEngaged")] ChangeResquests changeResquests)
        {
            if (id != changeResquests.Id)
            {
                return NotFound();
            }

			foreach (var i in Teams)
			{
				TeamList += i + ", ";
			}

			changeResquests.TeamsEngaged = TeamList;

			if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(changeResquests);
                    await _context.SaveChangesAsync();

					var user = await _userManager.GetUserAsync(User);
					EmailAddress fromAddress = new EmailAddress();
					EmailMessage emailMessage = new EmailMessage();
					StringBuilder msgBody = new StringBuilder();

					msgBody.Append("<p><div><p style=\"background-color: #ADD8E6\" align=\"center\">");
					msgBody.Append("<font size=5><b> " + changeResquests.Status + " - Planned Maintenance - CR#: " + changeResquests.Number + "</b></font></p></div><br/>");
					msgBody.Append("<font size=3><div><b>Subject:</b> " + changeResquests.Subject + "</div><br/>");
					msgBody.Append("<div><b>Summary: </b>" + changeResquests.Summary + "</div><br/>");
					msgBody.Append("<div><b>Start Date/Time: </b>" + changeResquests.StartDateTime + "</div><br/>");
					msgBody.Append("<div><b>Impact: </b>" + changeResquests.Impact + "</div><br/>");
					msgBody.Append("<div><b>Teams Engaged: </b>" + TeamList + "</div><br/>");
					msgBody.Append("<div><b>Endt Date/Time: </b>" + changeResquests.EndDateTime + "</div></font></p>");

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
					emailMessage.Subject = "" + changeResquests.Status + " - Planned Maintenance - CR#: " + changeResquests.Number;
					emailMessage.Content = msgBody.ToString();
					_emailService.Send(emailMessage);
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChangeResquestsExists(changeResquests.Id))
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
            return View(changeResquests);
        }

		// GET: ChangeResquests/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var changeResquests = await _context.ChangeResquests
                .SingleOrDefaultAsync(m => m.Id == id);
            if (changeResquests == null)
            {
                return NotFound();
            }

            return View(changeResquests);
        }

        // POST: ChangeResquests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var changeResquests = await _context.ChangeResquests.SingleOrDefaultAsync(m => m.Id == id);
            _context.ChangeResquests.Remove(changeResquests);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChangeResquestsExists(int id)
        {
            return _context.ChangeResquests.Any(e => e.Id == id);
        }
    }
}
