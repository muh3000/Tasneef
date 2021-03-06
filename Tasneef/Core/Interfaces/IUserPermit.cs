﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Core.Interfaces
{
    public interface IUserPermit
    {
        Task<Boolean> HasPermitOnCustomerAsync( int CustomerId);
        Task<Boolean> IsInRoleAsync(string Role);

        Task<List<int>> GetPermittedCustomersAsync();
    }
}
