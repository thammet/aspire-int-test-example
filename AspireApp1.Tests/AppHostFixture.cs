using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace AspireApp1.Tests;

public class AppHostFixture : IDisposable, IAsyncLifetime
{
    public DistributedApplication App;

    public void Dispose() => App.Dispose();

    public async Task DisposeAsync()
    {
        await App.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        var appHost = DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireApp1_AppHost>().Result;

        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        App = await appHost.BuildAsync();

        await App.StartAsync().WaitAsync(TimeSpan.FromMinutes(5));
    }
}