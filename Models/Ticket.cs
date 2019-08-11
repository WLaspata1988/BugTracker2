using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; } //Ticket ID number. example...Project ID of 1, ticket number 1. Parent Key of 1 -- child of 1.
        public int ProjectId { get; set; } //Project the ticket is assigned to
        public int TicketTypeId { get; set; } //Describe the type of ticket it is
        public int TicketPriorityId { get; set; } //Categorize ticket as Low/Moderate/High priority
        public int TicketStatusId { get; set; } // Ticket Status Open/Closed/Delayed?
        [StringLength(50, ErrorMessage = "Title must be between {2} and {1} characters long", MinimumLength = 5)]
        public string Title { get; set; } //Subject line/heading for ticket reason
        [StringLength(200, ErrorMessage = "Description must be between {2} and {1} characters long", MinimumLength = 5)]
        public string Description { get; set; } //Detailed information of the issue or reason in full of why it was submitted
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string OwnerId { get; set; } //Person who submitted the ticket
        public string AssignedToUserId { get; set; } //Developer(s) or user(s) who is in charge of handling the ticket.
        public bool Deleted { get; set; }//Ticket deleted


        //Virtual Navigation section

        public virtual Project Project { get; set; }//Project key used to reference a ticket type//name
        public virtual TicketType TicketType { get; set; } //TicketType key used to reference what type of ticket it is
        public virtual TicketPriority TicketPriority { get; set; }//TicketPriority key used to determined what priority level is for tickets
        public virtual TicketStatus TicketStatus { get; set; }//TicketStatus key to describe whether ticket is open or closed
        public virtual ApplicationUser Owner { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        public virtual ICollection<TicketPriority> TicketPriorities { get; set; }

        public Ticket()
        {
            TicketPriorities = new HashSet<TicketPriority>();
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();
        }
        /*public virtual OwnerId OwnerId { get; get; }*///OwnerId key of person who created ticket
        /*public virtual AssignedToUserId AssignedToUserId { get; set; }*///UserId of the user(s) or user assigned to the ticket

   

    }
}