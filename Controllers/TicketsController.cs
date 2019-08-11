using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        [Authorize]
        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.Owner).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }
        [Authorize(Roles ="Project Manager, Developer, Submitter")]
        public ActionResult MyIndex()
        {
            //MyIndex a view with only M tickets, If I am a submitter, they are my tickets.

            //Get ID of the logged in User
            //OwnerUserId = my Id. I am the developer.
            var userId = User.Identity.GetUserId();
            //Ask the question, "what role do I occupy?"
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var myTickets = new List<Ticket>();

            switch (myRole)
            {
                case "Developer":
                    myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "Submitter":
                    myTickets = db.Tickets.Where(t => t.OwnerId == userId).ToList();
                    break;
                case "Project Manager":
                    myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                    break;


            }
            return View("Index", myTickets);

        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                  

                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                ticket.OwnerId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

           
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Project Manager, Submitter, Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            var allowed = false;
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Developer") && ticket.AssignedToUserId == userId)
                allowed = true;
            else if (User.IsInRole("Submitter") && ticket.OwnerId == userId)
                allowed = true;
            else if (User.IsInRole("Project Manager"))
            {
                //if(db.Users.Find(userId).Projects.SelectMany(p => p.Tickets).Select(t => new { id = t.Id}).Contains(ticket.Id));
               //if this ticket is on a project I am on, then I can edit this ticket
            }
            else
            {
                allowed = true;
            }
            if(allowed)
            {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);   
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
            }
            else
            {
                return RedirectToAction("AccessViolation, Admin");
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,Title,Description,Created,Updated,OwnerId,AssignedToUserId,Deleted")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Dashboard(int id)
        {
            var ticket = db.Tickets.Find(id);

            if(ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
    }
}
