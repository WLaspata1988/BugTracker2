using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BugTracker.Helpers
{
    public class TicketsHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        public bool IsUserAssignedToTicket(string userId, int projectId, int ticketId)
        {

            var projects = db.Projects.Find(projectId);
            var tickets = db.Tickets.Find(ticketId);
            var flag = tickets.Project.Users.Any(u => u.Id == userId);
            return (flag);
        }


        public void AssignUserToTicket(string userId, int ticketId, int projectId)
        {
            if (!IsUserAssignedToTicket(userId, projectId, ticketId))
            {
                Ticket ticket = db.Tickets.Find(ticketId);
                Project proj = db.Projects.Find(userId);
                var newUser = db.Users.Find(userId);

                ticket.Project.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromTicket(string userId, int projectId, int ticketId)
        {
            if (IsUserAssignedToTicket(userId, projectId, ticketId))
            {
                Ticket ticket = db.Tickets.Find(ticketId);
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                ticket.Project.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> ListTicketsForUser(string ownerId, int projectId, string ticketId)
        {
            HttpContext.Current.User.Identity.GetUserId();
            Project proj = db.Projects.Find(projectId);
            Ticket ticket = db.Tickets.Find(ticketId);
            var tickets = ticket.Project.Users.ToList();      
            return (tickets);

        }

        public ICollection<ApplicationUser> UsersOnTicket(int ticketId)
        {
            return db.Tickets.Find(ticketId).Project.Users.ToList();
        }
        
        public List<string> UsersInRoleOnTicket(int ticketId, string roleName)
        {
            var people = new List<string>();
            foreach (var user in UsersOnTicket(ticketId).ToList())
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    people.Add(user.Id);
                }
            }
            return people;
        }
    }
}