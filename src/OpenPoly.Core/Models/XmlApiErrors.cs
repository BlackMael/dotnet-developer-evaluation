namespace OpenPoly.Core.Models;

public static class XmlApiErrors
{
    public static Error UnexpectedResult = new("XmlApi.UnexpectedResult", "Xml Api returned unexpected result");

    public static Error UnknownCompany(int id) => new("XmlApi.UnknownCompany", $"The company with Id '{id}' was not found");
    public static Error Exception(string message) => new("XmlApi.Exception", message);
}
