using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Tasneef.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Tasneef.Services
{
    public class AppHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<AppHostedService> _logger;
        private Timer _timer;
        //private readonly ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public AppHostedService(ILogger<AppHostedService> logger, IServiceScopeFactory scopeFactory)//, ApplicationDbContext context)
        {
            _logger = logger;
            //_context = context;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            //int i = _context.AppUsers.Count();
            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}");
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //do what you need

                var x = context.Customers.FirstOrDefault();
                if(x!=null)
                _logger.LogInformation("Timed Hosted Service is working. Name: {Count}", x.Name);  
            }


            //Console.WriteLine("Timed Hosted Service is working. " + _x.getValue());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    
}
