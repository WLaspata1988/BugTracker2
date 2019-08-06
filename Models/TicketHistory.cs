using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }//Reference for parent Ticket the history is for
        public string UserId { get; set; }//User who made the changes(?)
        public string PropertyName { get; set; }//I have no idea
        public string OldValue { get; set; }//State of the ticket before update?
        public string NewValue { get; set; }//State of the ticket after update?
        public DateTime Updated { get; set; }//Time/date when ticket was changed?


        public virtual Ticket Ticket { get; set; }//reference to primary key of ticket or parent ID
        public virtual ApplicationUser User { get; set; }//Reference to primary key of UserId who made the changes
    }
}