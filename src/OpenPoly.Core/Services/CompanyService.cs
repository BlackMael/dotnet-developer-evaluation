using OpenPoly.Core.ClientAPI.OpenApiXML;
using OpenPoly.Core.Models;
using OpenPoly.Core.Results;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace OpenPoly.Core.Services;

public interface ICompanyService
{
    Task<CompanyResult> GetAsync(int id);
}

public class CompanyService : ICompanyService
{
    private readonly OpenApiClient _client;

    public CompanyService(OpenApiClient client)
    {
        _client = client;
    }

    public async Task<CompanyResult> GetAsync(int id)
    {
        if (id < 1 || id > 2)
        {
            return CompanyResult.Failure(XmlApiErrors.UnknownCompany(id));
        }

        try
        {
            var stream = await _client.XmlApi.WithIdXml(id).GetAsync();

            if (stream is null)
            {
                return CompanyResult.Failure(XmlApiErrors.Exception("The response stream is null"));
            }

            using var reader = new StreamReader(stream);

            var xmlContent = await reader.ReadToEndAsync();

            // Deserialize the XML to Company object
            var serializer = new XmlSerializer(typeof(Company));

            using StringReader stringReader = new(xmlContent);

            if (serializer.Deserialize(stringReader) is not Company result)
            {
                return CompanyResult.Failure(XmlApiErrors.UnexpectedResult);
            }

            return CompanyResult.Success(result);
        }
        catch (InvalidOperationException ex)
        {
            return CompanyResult.Failure(XmlApiErrors.UnexpectedResult);
        }
        catch (Exception ex)
        {
            return CompanyResult.Failure(XmlApiErrors.Exception(ex.Message));
        }
    }
}
