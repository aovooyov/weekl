using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Weekl.Core.Helper;

namespace Weekl.Core.Models
{
    [Serializable]
    public class FilterXml
    {
        public FilterXml()
        {
            Sources = new List<int>();
        }

        [XmlAttribute]
        public string SourceUnique { get; set; }

        [XmlArray("Sources")]
        [XmlArrayItem("Source")]
        public List<int> Sources { get; set; }

        public static FilterXml Empty => new FilterXml();

        public static FilterXml BySource(string sourceUnique) => new FilterXml { SourceUnique = sourceUnique };
    }
}