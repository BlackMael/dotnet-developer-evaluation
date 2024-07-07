using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OpenPoly.Core.Models;
using OpenPoly.Core.Services;
using Http = Microsoft.AspNetCore.Http;

namespace OpenPoly.Core.Modules;

public class CompanyModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/companies/{id}",
            async (int id, ICompanyService service) =>
                {
                    var result = await service.GetAsync(id);

                    return result.Match(
                        onSuccess: value => Http.Results.Ok(value),
                        onFailure: error => Http.Results.NotFound(error));
                }
            )
            .WithTags("Companies")
            .WithName("GetCompanyById")
            .Produces<Company>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi(op =>
            {
                // Add examples for 200 OK response
                op.Responses[StatusCodes.Status200OK.ToString()].Content["application/json"].Examples =
                new Dictionary<string, OpenApiExample>
                {
                    ["OP"] = new OpenApiExample
                    {
                        Value = new OpenApiObject
                        {
                            ["id"] = new OpenApiInteger(1),
                            ["name"] = new OpenApiString("OP"),
                            ["description"] = new OpenApiString("..is awesome"),
                        }
                    },
                    ["Other"] = new OpenApiExample
                    {
                        Value = new OpenApiObject
                        {
                            ["id"] = new OpenApiInteger(2),
                            ["name"] = new OpenApiString("Oher"),
                            ["description"] = new OpenApiString("....is not"),
                        }
                    }
                };

                // Add example for 404 Not Found response
                op.Responses[StatusCodes.Status404NotFound.ToString()].Content["application/json"].Example =
                    new OpenApiObject
                    {
                        ["error"] = new OpenApiString("XmlApi.UnknownCompany"),
                        ["error_message"] = new OpenApiString("The company with Id '1' not found")
                    };
                return op;
            });
    }
}
