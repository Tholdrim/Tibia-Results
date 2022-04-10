using Microsoft.Extensions.Hosting;
using TibiaResults.Interfaces;

namespace TibiaResults.Services
{
    internal class ConsoleHostedService : IHostedService
    {
        private readonly Lazy<IApplicationService> _applicationServiceProvider;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public ConsoleHostedService(Lazy<IApplicationService> applicationServiceProvider, IHostApplicationLifetime applicationLifetime)
        {
            _applicationServiceProvider = applicationServiceProvider;
            _applicationLifetime = applicationLifetime;
        }

        private int? ExitCode { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    var output = await _applicationServiceProvider.Value.RunAsync();

                    Console.Write(output);

                    ExitCode = 0;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    ExitCode = -1;
                }
                finally
                {
                    _applicationLifetime.StopApplication();
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = ExitCode.GetValueOrDefault(-1);

            return Task.CompletedTask;
        }
    }
}
