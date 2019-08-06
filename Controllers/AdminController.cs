using BugTracker.Models;
using BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {        
            private ApplicationDbContext db = new ApplicationDbContext();
            private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        // GET: Admin
        public ActionResult UserIndex()
        {
            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
                }).ToList();

            return View(users);
        }
        //GET ManageUserRole
        public ActionResult ManageUserRole(string userId)
        {
            ViewBag.UserId = userId;
            ViewBag.UserRole = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View();
        }

        //POST ManageUserRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRole(string userId, string userRole)
        {
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            //if the incoming role selection is NOT NULL I want to assign the user to the selected role
            if (!string.IsNullOrEmpty(userRole))
            {
                roleHelper.AddUserToRole(userId, userRole);
            }

            return RedirectToAction("UserIndex");
        }

        
        
        //GET ManageRoles
        public ActionResult ManageRoles(string userId, string userRole)
        {
            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View();
        }

        //POST ManageRoles
        [HttpPost]
        public ActionResult ManageRoles(List<string> users, string roleName)
        {
            //Let's iterate over the incorming list of Users that were selected from the form
            //and remove each of them from whatever role they occupy only to add them back to the selected role

            foreach (var userId in users)
            {
                //Remove them from any tole they occupy
                foreach(var role in roleHelper.ListUserRoles(userId))
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }

                //Only to add back the selected role
                if(!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }

            return RedirectToAction("ManageRoles");
        }
        //GET
        public ActionResult ManageUserProjects(string userId)
        {
            var myProjects = projectHelper.ListProjectsForUser(userId).Select(p => p.Id);
            ViewBag.Projects = new MultiSelectList(db.Projects.ToList(), "Id", "Name", myProjects);
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(List<int>projects, string userId)
        {
            foreach (var project in projectHelper.ListProjectsForUser(userId).ToList())
            {
                projectHelper.RemoveUserFromProject(userId, project.Id);
            }
           if (projects != null)
            {
                foreach(var projectId in projects)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }
            return RedirectToAction("ManageUserProjects");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(int projectId, List<string>ProjectManagers, List<string>Developers, List<string>Submitters)
        {
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }
            if(ProjectManagers != null)
            {
                foreach (var projectManagerId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(projectManagerId, projectId);
                }
            }
            if(Developers != null)
            {
                foreach(var developerId in Developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }
            if (Submitters != null)
            {
                foreach (var submitterId in Submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }
                return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        public ActionResult RemoveProjectUser(string userId, int projectId)
        {
            projectHelper.RemoveUserFromProject(userId, projectId);
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }
    } 
}