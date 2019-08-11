using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using BugTracker.Enumerations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Helpers
{
    
    public class DecisionHelper
    {
       
        private static UserRolesHelper roleHelper = new UserRolesHelper();
       private static ApplicationDbContext db = new ApplicationDbContext();
        private static ProjectsHelper projectHelper = new ProjectsHelper();

        public static bool TicketIsDetailIsViewableByUser(string userId, int ticketId)
        {
            //var userId = HttpContext.Current.User.Identity.GetUserId();

            var roleName = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var systemRole = (SystemRole)Enum.Parse(typeof(SystemRole), roleName);

            switch (systemRole)
            {
                case SystemRole.Admin:
                    break;
                case SystemRole.ProjectManager:
                    break;
                case SystemRole.Developer:
                    break;
                case SystemRole.Submitter:
                    break;
            }


            return true;
        }
        public static bool TicketIsEditableByUser(Ticket ticket)
        {
            

           var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            

            switch (myRole)
            {
                case "Developer":
                    return ticket.AssignedToUserId == userId;
                case "Submitter":
                    return ticket.OwnerId == userId;
                case "Project Manager":
                    var myProjects = projectHelper.ListUserProjects(userId);
                    foreach(var project in myProjects)
                    {
                        foreach(var projticket in project.Tickets)
                        {
                            if (projticket.Id == ticket.Id)
                                return true;
                        }
                    }
                    return false;                 
                case "Admin":
                    return true;
                default:
                    return false;
                
            }      

        }
        public bool TicketTypeIsEditableByUser(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.OwnerId == userId;
        }
        public bool TicketStatusIsEditableByUser(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.OwnerId == userId;
        }
        public bool TicketPriorityIsEditableByUser(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.OwnerId == userId;
        }
        public bool TicketTitleIsEditableByUser(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.OwnerId == userId;
        }
        public bool TicketDescriptionIsEditableByUser(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.OwnerId == userId;
        }
        public bool TicketAssignedToUserIdIsEditableByUser(string userId, int ticketId)
        {
            return true;
        }
        public bool IsDevAssignedToTicket(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            return ticket.AssignedToUserId == userId;
        }


        public void AssignDevToTicket(string userId, int ticketId)
        {
            if(!IsDevAssignedToTicket(userId, ticketId))
            {
                Ticket ticket = db.Tickets.Find(ticketId);
                var newDev = roleHelper.IsUserInRole(userId, "Developer");

                ticket.AssignedToUserId.ToList();
                db.SaveChanges();
                 
            }
            
        }

        public static List<Ticket> ListTicketsForDeveloper(string userId)
        {
            HttpContext.Current.User.Identity.GetUserId();
            var tickets = new List<Ticket>();
            return (tickets);
        }
        


    }
}