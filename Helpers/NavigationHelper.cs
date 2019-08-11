using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class NavigationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserRolesHelper roleHelper = new UserRolesHelper();

        //public static List<Project> ListUserProjects(string userId)
        //{

        //}












    //public static List<Ticket> ListUserTickets(string userId)
    //{
    //    if (string.IsNullOrEmpty(userId))
    //        return new List<Ticket>();

    //    var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
    //}



    }
}