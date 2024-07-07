using Microsoft.Kiota.Abstractions;

namespace OpenPoly.Core.ClientAPI.OpenApiXML.XmlApi.WithIdXml;

public interface IWithIdXmlRequestBuilder
{
    Task<Stream?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default);
}

public partial class WithIdXmlRequestBuilder : IWithIdXmlRequestBuilder
{
}
