using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        //Parent Names
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }


        //Virtual nav
        public virtual ICollection<Ticket> Tickets { get; set; }//Icollection List/HashSet = Interface can be satisfied with more than 1 type collection = Collection of a certain type "<Tickets>" Tickets section
        public virtual ICollection<ApplicationUser> Users { get; set; }
        


        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}