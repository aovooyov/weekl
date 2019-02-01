using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Weekl.Core.Entities.Feed;
using Weekl.Core.Models;

namespace Weekl.Core.Helper
{
    public static class XmlHelper
    {
        public static string SourcesToXml(IEnumerable<SourceXml> sources)
        {
            var builder = new StringBuilder("<Source>");

            foreach (var source in sources)
            {
                builder.AppendLine(ChannelToXml(source.Name, source.Source, source.Channel, source.Selector, source.Encoding));
            }

            builder.AppendLine("</Source>");

            return builder.ToString();
        }

        public static string ChannelToXml(string name, string source, string channel, string selector, string encoding)
        {
            return $"<Channel Name=\"{name}\" Source=\"{source}\" Channel=\"{channel}\" Selector=\"{selector}\" Encoding=\"{encoding}\"></Channel>";
        }

        public static string ArticlesToXml(IEnumerable<ArticleXml> articles)
        {
            var formatter = new XmlSerializer(typeof(ArticlesXml));

            using (var stringWriter = new StringWriter())
            {
                formatter.Serialize(stringWriter, new ArticlesXml { Articles = articles.ToArray() });
                return stringWriter.ToString();
            }
        }

        public static string FilterToXml(FilterXml filter)
        {
            var formatter = new XmlSerializer(typeof(FilterXml));

            using (var stringWriter = new StringWriter())
            {
                formatter.Serialize(stringWriter, filter);
                return stringWriter.ToString();
            }
        }
    }
}