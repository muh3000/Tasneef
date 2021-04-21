using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Core.Interfaces
{
    public interface IDashboard
    {
        Task<int> GetNumberOfActiveProjects();
        Task<int> GetNumberOfNotifications();

        Task<int> GetNumberOfPendingUploads();

        Task<int> GetNumberofActiveSubscriptions();


    }
}
