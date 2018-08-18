using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Weekl.Core.Models
{
    [Serializable]
    public class ArticleXml
    {
        [XmlAttribute]
        public int ChannelId { get; set; }
    
        [XmlAttribute]
        public DateTime Date { get; set; }
        
        [XmlAttribute]
        public string Link { get; set; }

        [XmlElement]
        public string Title { get; set; }

        [XmlElement]
        public string SubTitle { get; set; }

        [XmlIgnore]
        public string Description { get; set; }

        [XmlElement("Description")]
        public XmlNode[] DescriptionNode
        {
            get
            {
                return new XmlNode[] { new XmlDocument().CreateCDataSection(Description) };
            }
            set { }
        }

        [XmlIgnore]
        public string Text { get; set; }

        [XmlElement("Text")]
        public XmlNode[] TextNode
        {
            get
            {
                return new XmlNode[] { new XmlDocument().CreateCDataSection(Text) };
            }
            set { }
        }

        [XmlIgnore]
        public string Category { get; set; }

        [XmlArray("Images")]
        [XmlArrayItem("Image")]
        public List<string> Images { get; set; }

        public ArticleXml()
        {
            Images = new List<string>();
        }
    }

    [Serializable]
    public class ArticlesXml
    {
        [XmlArray("Articles")]
        [XmlArrayItem("Article")]
        public ArticleXml[] Articles { get; set; }
    }
}