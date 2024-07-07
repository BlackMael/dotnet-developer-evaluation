using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using OpenPoly.Core.ClientAPI.OpenApiXML;
using OpenPoly.Core.ClientAPI.OpenApiXML.XmlApi;
using OpenPoly.Core.ClientAPI.OpenApiXML.XmlApi.WithIdXml;
using OpenPoly.Core.Models;
using OpenPoly.Core.Services;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace OpenPoly.Core.Tests;

public class CompanyServiceTests
{
    [Theory]
    [InlineData(-99)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(42)]
    public async Task Given_id_is_out_of_bounds_return_failure(int id)
    {
        // Arrange
        var httpClient = new HttpClient();
        var client = (new OpenApiClientFactory(httpClient)).GetClient();

        var service = new CompanyService(client);

        // Act
        var result = await service.GetAsync(id);

        // Assert
        result.IsSuccess.ShouldBe(false);
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.error.ShouldBe("XmlApi.UnknownCompany");
    }

    [Theory]
    [InlineData(1, "OpenPolytechnic", "..is awesome")]
    [InlineData(2, "Other", "....is not")]
    public async Task Given_valid_id_return_success(int id, string name, string description)
    {
        // Arrange
        var httpClient = new HttpClient();

        httpClient.BaseAddress = new Uri("https://raw.githubusercontent.com/openpolytechnic/dotnet-developer-evaluation/main");

        var client = (new OpenApiClientFactory(httpClient)).GetClient();

        var service = new CompanyService(client);

        // Act
        var result = await service.GetAsync(id);

        // Assert
        result.IsSuccess.ShouldBe(true);
        result.Value.ShouldNotBeNull();
        result.Error.ShouldBeNull();

        var company = result.Value;

        company.Id.ShouldBe(id);
        company.Name.ShouldBe(name);
        company.Description.ShouldBe(description);
    }

    [Fact]
    public async Task When_api_is_down_return_failure_with_exception()
    {
        // Arrange

        // FULL DISCLOSURE
        // This generated class was altered manually to allow the following to be mocked successfully
        // OpenApiClient.XmlApi property was made virtual
        // XmlApiRequestBuilder.WithIdXml method was made virtual
        // WithIdXmlRequestBuilder.GetAsync method was made virtual

        IRequestAdapter adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: new HttpClient());

        var pathParameters = new Dictionary<string, object>();
        var id = 1.0;

        var mockWithIdBuilder = Substitute.For<WithIdXmlRequestBuilder>(pathParameters, adapter, id);
        mockWithIdBuilder.GetAsync().Returns<Task<Stream?>>(x =>
        {
            throw new Exception("Mock");
        });

        var mockApiBuilder = Substitute.For<XmlApiRequestBuilder>(pathParameters, adapter);
        mockApiBuilder.WithIdXml(Arg.Any<double?>()).Returns(mockWithIdBuilder);

        var mockClient = Substitute.For<OpenApiClient>(adapter);
        mockClient.XmlApi.Returns(mockApiBuilder);

        var service = new CompanyService(mockClient);

        // Act
        
        var result = await service.GetAsync(1);

        // Assert

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.error.ShouldBe("XmlApi.Exception");
        result.Error.error_message.ShouldBe("Mock");
    }

    [Fact]
    public async Task When_WithIdXml_resopnse_is_null_return_failure_with_exception()
    {
        // Arrange

        // FULL DISCLOSURE
        // This generated class was altered manually to allow the following to be mocked successfully
        // OpenApiClient.XmlApi property was made virtual
        // XmlApiRequestBuilder.WithIdXml method was made virtual
        // WithIdXmlRequestBuilder.GetAsync method was made virtual

        IRequestAdapter adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: new HttpClient());

        var pathParameters = new Dictionary<string, object>();
        var id = 1.0;

        var mockWithIdBuilder = Substitute.For<WithIdXmlRequestBuilder>(pathParameters, adapter, id);
        mockWithIdBuilder.GetAsync().ReturnsNull();

        var mockApiBuilder = Substitute.For<XmlApiRequestBuilder>(pathParameters, adapter);
        mockApiBuilder.WithIdXml(Arg.Any<double?>()).Returns(mockWithIdBuilder);

        var mockClient = Substitute.For<OpenApiClient>(adapter);
        mockClient.XmlApi.Returns(mockApiBuilder);

        var service = new CompanyService(mockClient);

        // Act

        var result = await service.GetAsync(1);

        // Assert

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.error.ShouldBe("XmlApi.Exception");
        result.Error.error_message.ShouldBe("The response stream is null");
    }

    [Fact]
    public async Task When_WithIdXml_resopnse_not_expected_xml_return_failure_with_unexpected_result()
    {
        // Arrange

        // FULL DISCLOSURE
        // This generated class was altered manually to allow the following to be mocked successfully
        // OpenApiClient.XmlApi property was made virtual
        // XmlApiRequestBuilder.WithIdXml method was made virtual
        // WithIdXmlRequestBuilder.GetAsync method was made virtual

        IRequestAdapter adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: new HttpClient());

        var pathParameters = new Dictionary<string, object>();
        var id = 1.0;

        var memory = new MemoryStream();
        var data = Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<BrokenData>\r\n\t<id>1</id>\r\n\t<name>OpenPolytechnic</name>\r\n\t<description>..is awesome</description>\r\n</BrokenData>");

        memory.Write(data, 0, data.Length);
        memory.Seek(0, SeekOrigin.Begin);
        
        var mockWithIdBuilder = Substitute.For<WithIdXmlRequestBuilder>(pathParameters, adapter, id);
        mockWithIdBuilder.GetAsync().Returns(memory);

        var mockApiBuilder = Substitute.For<XmlApiRequestBuilder>(pathParameters, adapter);
        mockApiBuilder.WithIdXml(Arg.Any<double?>()).Returns(mockWithIdBuilder);

        var mockClient = Substitute.For<OpenApiClient>(adapter);
        mockClient.XmlApi.Returns(mockApiBuilder);

        var service = new CompanyService(mockClient);

        // Act

        var result = await service.GetAsync(1);

        // Assert

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();
        result.Error.ShouldNotBeNull();
        result.Error.error.ShouldBe("XmlApi.UnexpectedResult");
        result.Error.error_message.ShouldBe("Xml Api returned unexpected result");
    }
}
