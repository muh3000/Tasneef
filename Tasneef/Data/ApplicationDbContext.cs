using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasneef.Models;
using Tasneef.ViewModels;

namespace Tasneef.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { set; get;}
        public DbSet<Customer> Customers { set; get; }
        public DbSet<Message> Messages { set; get; }
        public DbSet<Project> Projects { set; get; }
        public DbSet<ProjectStatus> ProjectStatuses { set; get; }
        public DbSet<Upload> Uploads { set; get; }
        public DbSet<AppSetting> AppSettings { set; get; }
        




    }
}
