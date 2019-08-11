using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using BugTracker.ViewModels;
using BugTracker.Models;

namespace BugTracker.Enumerations
{
    public enum SystemRole
    {
        Admin,
        ProjectManager,
            Submitter,
            Developer
    }
    


}