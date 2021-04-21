using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasneef.Core.Interfaces;
using Tasneef.Data;
using Tasneef.Models;

using Microsoft.EntityFrameworkCore;

namespace Tasneef.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNotificationAsync(Notification item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.Id;
        }


        public async Task DeleteNotificationAsync(Notification item)
        {
            _context.Notifications.Remove(item);
            await _context.SaveChangesAsync();
        }


        public async Task<Notification> GetNotificationAsync(int id)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            

            return notification;
        }

        public async Task<List<Notification>> GetNotificationsListAsync()
        {
            return await _context.Notifications.ToListAsync();
        }
                

        public async Task MarkNotificationAsReadAsync(Notification item)
        {
            var notification = await GetNotificationAsync(item.Id);
            notification.Read = true;
            notification.ReadDate = DateTime.Now;
            _context.Update(notification);
            await _context.SaveChangesAsync();
        }

        

        public async Task UpdateNotificationAsync(Notification item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateCustomerNotificationsAsync(String Entity, String EntityId, Customer customer, string Title)
        {
            foreach (var customerUser in customer.CustomerUsers)
            {
                await AddNotificationAsync(new Notification()
                {
                    Title = Title,
                    CreatedDate = DateTime.Now,
                    Entity = Entity,
                    EntityId = EntityId,
                    UserId = customerUser.UserId
                });
            }

            return 0;
        }

        public async Task<int> DeleteCustomerNotificationsAsync(string Entity, string EntityId, Customer customer)
        {
            if (customer != null)
            {

                var customerUsersList = _context.CustomerUsers.Where(c => c.CustomerId == customer.Id);
                foreach (var customerUser in customerUsersList)
                {
                    var notes = _context.Notifications.Where(n => n.Entity == Entity && n.EntityId == EntityId && n.UserId == customerUser.UserId);
                    foreach (var item in notes)
                    {
                        _context.Notifications.Remove(item);
                        
                    }

                }
            }
            await _context.SaveChangesAsync();
            return 0;
        }
    }
}
