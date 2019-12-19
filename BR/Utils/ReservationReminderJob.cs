using BR.EF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BR.Services;

namespace BR.Utils
{
    [DisallowConcurrentExecution]
    public class ReservationReminderJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ReservationReminderJob> _logger;
        public ReservationReminderJob(IServiceProvider provider, 
            ILogger<ReservationReminderJob> logger)
        {
            _provider = provider;
            _logger = logger;

        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<BRDbContext>();
                var userService  = scope.ServiceProvider.GetService<IWaiterService>();
                var waiter = userService.GetWaiter(1).Result;
                _logger.LogInformation(waiter.FirstName);
                // fetch customers, send email, update DB
            }

            return Task.CompletedTask;
        }
    }
}
