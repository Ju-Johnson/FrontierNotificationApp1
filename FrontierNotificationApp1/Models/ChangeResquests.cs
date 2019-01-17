using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontierNotificationApp1.Models
{
    public partial class ChangeResquests
    {
        public int Id { get; set; }
		public string Number { get; set; }
        public string Subject { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Impact { get; set; }
		public string TeamsEngaged { get; set; }
	}
}
