using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontierNotificationApp1.Models
{
    public partial class Incidents
    {
		
        public int Id { get; set; }
        public string Priority { get; set; }
        public string Sdpticket { get; set; }
        public string Status { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string Subject { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime InitialDateTime { get; set; }
        public string Impact { get; set; }
        public string VendorTicket { get; set; }
		public string TeamsEngaged { get; set; }
	}



}
