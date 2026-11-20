using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clubmanager.Areas.Admin.Models; 

namespace clubmanager.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<tblMenu> Menus { get; set; }
        public DbSet<tblUser> User { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }    
    }
}