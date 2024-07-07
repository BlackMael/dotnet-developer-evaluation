using OpenPoly.Core.ClientAPI.OpenApiXML.XmlApi.WithIdXml;

namespace OpenPoly.Core.ClientAPI.OpenApiXML.XmlApi;

public interface IXmlApiRequestBuilder
{
    WithIdXmlRequestBuilder WithIdXml(double? id);
}

public partial class XmlApiRequestBuilder : IXmlApiRequestBuilder
{
}
