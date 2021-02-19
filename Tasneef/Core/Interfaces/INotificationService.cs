using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasneef.Models;

namespace Tasneef.Core.Interfaces
{
    public interface INotificationService
    {
        Task<int> AddNotificationAsync(Notification item);
        Task UpdateNotificationAsync(Notification item);
        Task DeleteNotificationAsync(Notification item);

        Task<List<Notification>> GetNotificationsListAsync();
        Task<Notification> GetNotificationAsync(int id);

        Task MarkNotificationAsReadAsync(Notification item);
        Task<int> CreateCustomerNotificationsAsync(String Entity, String EntityId, Customer customer, string Title);

    }
}
