using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasneef.Models;
using Tasneef.ViewModels;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tasneef.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly UserManager<AppUser> _userManager;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)//, UserManager<AppUser> userManager)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            //_userManager = userManager;
        }
        public DbSet<AppUser> AppUsers { set; get;}
        public DbSet<Customer> Customers { set; get; }
        public DbSet<CustomerUser> CustomerUsers { set; get; }
        public DbSet<Message> Messages { set; get; }
        public DbSet<Project> Projects { set; get; }
        public DbSet<ProjectStatus> ProjectStatuses { set; get; }
        public DbSet<Upload> Uploads { set; get; }
        public DbSet<AppSetting> AppSettings { set; get; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<CustomerMemo> CustomerMemos { get; set; }
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }
        public DbSet<AuditTrail> AuditTrails { set; get; }
        public DbSet<Notification> Notifications { set; get; }

        public DbSet<EmployeeCustomerGrant> EmployeeCustomerGrants { set; get; }
        public override int  SaveChanges()
        {
            //var task = Task.Run(async () => {

            //    await AddTimestampsAsync();

            //});

            
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await AuditEntityAsync();
            return await base.SaveChangesAsync();
        }



        //private async Task AddTimestampsAsync()
        //{
        //    var entities = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        //    var currentUsername = !string.IsNullOrEmpty( _httpContextAccessor.HttpContext.User?.Identity?.Name)
        //        ? _httpContextAccessor.HttpContext.User.Identity.Name
        //        : "Anonymous";
            
        //    var user = await AppUsers.FirstOrDefaultAsync(u => u.Email == currentUsername);

        //    string userId = user!=null?user.Id: "";
            
                       

        //    foreach (var entity in entities)
        //    {
        //        if (entity.State == EntityState.Added)
        //        {
        //            ((AuditableEntity)entity.Entity).CreatedDate = DateTime.Now;
        //            ((AuditableEntity)entity.Entity).CreatedById = userId;
        //        }

        //        ((AuditableEntity)entity.Entity).UpdatedDate = DateTime.Now;
        //        ((AuditableEntity)entity.Entity).UpdatedById = userId;
        //    }
        //}

        private async Task AuditEntityAsync()
        {
            ChangeTracker.DetectChanges();

            foreach (EntityEntry entry in ChangeTracker.Entries().ToList())
            {
                if (entry.Entity is AuditTrail || entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }


                var TableName = entry.Metadata.GetTableName();
                string keyValue = "";
                foreach (PropertyEntry property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        if (keyValue != "")
                        {
                            keyValue += ";";
                        }
                        keyValue += property.CurrentValue.ToString();
                        continue;
                    }
                }

                foreach (PropertyEntry property in entry.Properties)
                {
                    if (property.CurrentValue != null)
                    {
                        string propertyName = property.Metadata.Name;
                        string dbColumnName = property.Metadata.GetColumnName();
                        AuditTrail audit = new AuditTrail();

                        audit.ChangedColumn = dbColumnName;
                        audit.KeyValues = keyValue;
                        if (property.Metadata.IsPrimaryKey())
                            continue;

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                audit.NewValue = property.CurrentValue.ToString();
                                audit.AuditType = AuditTrailType.Create.ToString();
                                break;

                            case EntityState.Deleted:
                                audit.OldValue = property.OriginalValue.ToString();
                                audit.AuditType = AuditTrailType.Delete.ToString();
                                break;

                            case EntityState.Modified:
                                if (property.IsModified)
                                {


                                    audit.NewValue = property.OriginalValue?.ToString();
                                    audit.OldValue = property.CurrentValue.ToString();

                                    if (audit.NewValue == audit.OldValue)
                                        continue;
                                    audit.AuditType = AuditTrailType.Update.ToString();
                                }
                                break;



                        }
                        var currentUsername = !string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.Identity?.Name)
                            ? _httpContextAccessor.HttpContext.User.Identity.Name : "Anonymous";

                        var user = await AppUsers.FirstOrDefaultAsync(u => u.Email == currentUsername);

                        string userId = user != null ? user.Id : currentUsername;
                        audit.AuditDateTime = DateTime.Now;
                        audit.TableName = TableName;
                        audit.AuditUser = userId;
                        AuditTrails.Add(audit);

                    }
                }



            }
        }
    }

    
}
