# SOLUTION

## Install

Install Visual Studio extension [REST API Client Code Generator](https://github.com/christianhelle/apiclientcodegen)

``` shell
dotnet tool install --global Microsoft.OpenApi.Kiota

```

Will also need to explicitly install project dependencies
``` shell
   dotnet add package Microsoft.Kiota.Abstractions --version 1.9.7
   dotnet add package Microsoft.Kiota.Authentication.Azure --version 1.1.7
   dotnet add package Microsoft.Kiota.Http.HttpClientLibrary --version 1.4.3
   dotnet add package Microsoft.Kiota.Serialization.Form --version 1.2.5
   dotnet add package Microsoft.Kiota.Serialization.Json --version 1.3.3
   dotnet add package Microsoft.Kiota.Serialization.Multipart --version 1.1.5
   dotnet add package Microsoft.Kiota.Serialization.Text --version 1.2.2
```
Although the Visual Studio extension di this too :/


Run tool to genrate the code in the Core project

``` shell
kiota generate -l CSharp -c OpenApiClient -n OpenPoly.Core.ClientAPI.OpenApiXML -d .\ClientAPI\OpenApiXML\openapi-xml.yaml -o .\ClientAPI\OpenApiXML
```

## Full Disclosure

The generated code has been manually altered to allow the mocking for use one key property and two key methods.

```
OpenApiClient.XmlApi property was made virtual
XmlApiRequestBuilder.WithIdXml method was made virtual
WithIdXmlRequestBuilder.GetAsync method was made virtual
```
