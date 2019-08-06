using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }


        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public string NotificationBody { get; set; }
        public bool IsRead { get; set; }

        //Nav
        public virtual Ticket Ticket { get; set; }//TicketId reference
        public virtual ApplicationUser Recipient { get; set; }//UserId Reference
        public virtual ApplicationUser Sender { get; set; }
    }
}