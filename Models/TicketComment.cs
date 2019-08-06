using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }//Id number of comment paired to a ticket of a project. Grandchild of project, child of Tickets. Project/Tickets/TicketComments --- 1/1/1
        public int TicketId { get; set; }// Parent ID Key, TicketID of 1, ticket comment ID of 1.
        public string AuthorId { get; set; }
        public string Comment { get; set; }//Body of the comment
        public DateTime Created { get; set; }//Time / date of creation MM d, yy
        public string UserId { get; set; }//UserId/Username of person who made comment

        public virtual Ticket Ticket { get; set; }//Reference to the tickets model to gain access to the ticket Id
        public virtual ApplicationUser Author { get; set; }//Reference to the ApplicationUser database to gain UserId
    }
}