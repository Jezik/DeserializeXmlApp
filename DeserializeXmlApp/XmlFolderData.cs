using System.Xml.Serialization;
using System.Collections.Generic;

namespace DeserializeXmlApp
{
    // ENum with valid members for calculation operations
    public enum Operands
    {   [XmlEnum("divide")]
        divide,
        [XmlEnum("multiply")]
        multiply,
        [XmlEnum("add")]
        add,
        [XmlEnum("subtract")]
        subtract
    };

    // Root <folder>
    [XmlRoot("folder")]
    public class FolderMain
    {
        [XmlElement("folder")]
        public List<Folder> Folder { get; set; }
    }

    // <folder name="calculation">
    public class Folder
    {
        [XmlAttribute("calculation")]
        public string Calculation { get; set; }
        [XmlElement("str", Order = 1)]
        public StrFirst StrFirst { get; set; }
        [XmlElement("str", Order = 2)]
        public StrSecond StrSecond { get; set; }
        [XmlElement("int", Order = 3)]
        public Mod Mod { get; set; }
    }

    // First <str> tag
    public class StrFirst
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    // Second <str> tag
    public class StrSecond
    {        
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public Operands Operand { get; set; }
    }    

    // <int>
    public class Mod
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
