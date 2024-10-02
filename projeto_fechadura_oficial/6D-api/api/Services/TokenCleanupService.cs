using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using _6D.DAO;

namespace _6D.Services
{
    /// <summary>
    /// Hosted service that periodically cleans up expired tokens from the database.
    /// </summary>
    public class TokenCleanupService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCleanupService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Service scope factory.</param>
        public TokenCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Starts the token cleanup service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous start operation.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Schedule the task to run every hour
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            // Create a new scope
            using (var scope = _scopeFactory.CreateScope())
            {
                var tokenDAO = scope.ServiceProvider.GetRequiredService<TokenDAO>();
                // Perform cleanup operations with tokenDAO
                tokenDAO.CleanupTokens();
            }
        }

        /// <summary>
        /// Stops the token cleanup service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous stop operation.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes the timer resource.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
