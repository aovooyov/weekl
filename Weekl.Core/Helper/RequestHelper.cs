using System;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Weekl.Core.Helper
{
    public static class RequestHelper
    {
        public static string Get(string url, string encoding)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpWebResponse webResponse;
            try
            {
                webResponse = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                webResponse = ex.Response as HttpWebResponse;
            }

            var streamResponse = webResponse?.GetResponseStream();
            string result;

            using (var streamReader = new StreamReader(streamResponse, Encoding.GetEncoding(encoding)))
            {
                result = streamReader.ReadToEnd();

                streamResponse.Close();
                streamReader.Close();
                webResponse.Close();
            }

            return result;
        }

        private static string ConvertWin1251ToUTF8(string source)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;
        }
    }
}