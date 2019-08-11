namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            #region roleManager

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            #endregion

            #region Assign User Roles

            //create users that will occupy roles of either admin or moderator

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "JTwichell@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JTwichell@mailinator.com",
                    Email = "JTwichell@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "twich"

                }, "abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "WillLaspata@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "WillLaspata@gmail.com",
                    Email = "WillLaspata@gmail.com",
                    FirstName = "Will",
                    LastName = "Laspata",
                    DisplayName = "WillPasta"

                }, "abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "xelimination@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "xelimination@gmail.com",
                    Email = "xelimination@gmail.com",
                    FirstName = "William",
                    LastName = "Laspata",
                    DisplayName = "Will"

                }, "abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "xelimination@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "xelimination@mailinator.com",
                    Email = "xelimination@mailinator.com",
                    FirstName = "Joe",
                    LastName = "Schmoe",
                    DisplayName = "JoeJoe"

                }, "abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "WillLaspata@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "WillLaspata@mailinator.com",
                    Email = "WillLaspata@gmail.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    DisplayName = "JaneJane"

                }, "abc&123!");
            }
            // how to assign user and moderator

            var userId = userManager.FindByEmail("xelimination@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("JTwichell@mailinator.com").Id;
            userManager.AddToRole(userId, "Moderator");


            #endregion Assign User Roles

            context.Projects.AddOrUpdate(p => p.Name,
            new Project { Name = "Bug Tracker", Description = "This is the Spock Bug Tracker project that is now out in the wild", Created = DateTime.Now });

            context.TicketPriorities.AddOrUpdate(t => t.Name,
                new TicketPriority { Name = "Immediate", Description = "Highest priority level, requiring immediate action." },
                new TicketPriority { Name = "High", Description = "A high priority level requiring quick action" },
                new TicketPriority { Name = "Medium", Description = "" },
                new TicketPriority { Name = "Low", Description = "" },
                new TicketPriority { Name = "None", Description = "" }
                );

            context.TicketStatuses.AddOrUpdate(t => t.Name,
            new TicketStatus { Name = "Unassigned", Description = "" },
            new TicketStatus { Name = "New/Unassigned", Description = "" },
            new TicketStatus { Name = "Assigned", Description = "" },
            new TicketStatus { Name = "In Progress", Description = "" },
            new TicketStatus { Name = "Completed", Description = "" },
            new TicketStatus { Name = "Archived", Description = "" }
            );

            context.TicketTypes.AddOrUpdate(t => t.Name,
            new TicketType { Name = "Bug", Description = "An error has occured that resulted in either the application crashing or the user seeing error information" },
            new TicketType { Name = "Defect", Description = "An error has occured that resulted in either a miscalculation or an incorrect workflow" },
            new TicketType { Name = "Feature Request", Description = "A client has called in asking for new functionality in an existing application" },
            new TicketType { Name = "Documentation Request", Description = "A client has called in asking for new documentation to be created for the existing application" },
            new TicketType { Name = "Training Request", Description = "A client has called in asking to schedule a training session" },
            new TicketType { Name = "Complaint", Description = "A client has called in to make a general complaint about our application" },
            new TicketType { Name = "Other", Description = "A call has been received that required follow up but is outside the usual types" }
                );
        }
    }
}
