using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Weekl.Core.Models
{
    [Serializable]
    public class ArticleImageXml
    {
        [XmlAttribute]
        public string Link { get; set; }
    }

    [Serializable]
    public class AricleImagesXml
    {
        [XmlArray("Images")]
        [XmlArrayItem("Image")]
        public List<string> Images { get; set; }

        public AricleImagesXml()
        {
            Images = new List<string>();
        }
    }
}