using System.Xml.Serialization;

namespace OpenPoly.Core.Models;

[XmlRoot("Data")]
public class Company
{
    [XmlElement("id")]
    public required int Id { get; set; }
    [XmlElement("name")]
    public required string Name { get; set; }
    [XmlElement("description")]
    public required string Description { get; set; }
}
