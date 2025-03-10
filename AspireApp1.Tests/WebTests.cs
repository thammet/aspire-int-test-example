using Microsoft.Extensions.Logging;

namespace AspireApp1.Tests;

public class WebTests : IClassFixture<AppHostFixture>
{
    readonly AppHostFixture Fixture;

    public WebTests(AppHostFixture fixture) 
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task WeatherForecast_200() 
    {
        var httpClient = Fixture.App.CreateHttpClient("apiservice");
        await Fixture.App.ResourceNotifications.WaitForResourceHealthyAsync("apiservice").WaitAsync(TimeSpan.FromSeconds(30));

        var response = await httpClient.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Frontend_MainPage_200()
    {
        var httpClient = Fixture.App.CreateHttpClient("webfrontend");
        await Fixture.App.ResourceNotifications.WaitForResourceHealthyAsync("webfrontend").WaitAsync(TimeSpan.FromSeconds(30));
        
        var response = await httpClient.GetAsync("/");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
