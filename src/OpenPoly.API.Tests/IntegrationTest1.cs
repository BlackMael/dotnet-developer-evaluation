using Shouldly;
using System.Net;

namespace OpenPoly.API.Tests.Tests;

public class IntegrationTest1
{
    [Fact]
    public async Task Get_company_1_returns_correct_json()
    {
        // Arrange
        
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.OpenPoly_API_AppHost>();
        
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        
        var httpClient = app.CreateHttpClient("apiservice");
        var response = await httpClient.GetAsync("/companies/1");

        // Assert

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var stream = await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);

        var json = await reader.ReadToEndAsync();

        json.ShouldBe("{\"id\":1,\"name\":\"OpenPolytechnic\",\"description\":\"..is awesome\"}");
    }
}
