using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TibiaResults.Extensions;
using TibiaResults.Formatters;
using TibiaResults.Interfaces;
using TibiaResults.Services;

var builder = new HostBuilder();

builder.ConfigureServices(services =>
{
    services
        .AddHostedService<ConsoleHostedService>()
        .AddApplicationService<ApplicationService>();

    services
        .AddSingleton<IConfigurationService, ConfigurationService>()
        .AddSingleton<IHighscoreRetrievalService, HighscoreRetrievalService>()
        .AddSingleton<ILevelTrackingService, LevelTrackingService>()
        .AddSingleton<IResultComputingService, ResultComputingService>();

    services
        .AddSingleton<IResultFormatter, DiscordFormatter>();
});

await builder.RunConsoleAsync();
