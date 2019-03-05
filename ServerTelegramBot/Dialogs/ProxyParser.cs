using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;

namespace Dialogs
{
    class ProxyParser
    {
        string _html;
        public ProxyParser(string uri)
        {
            _html = uri;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResponse(_html));
            var t = doc.DocumentNode.FirstChild;
            var c = doc.DocumentNode.SelectSingleNode(@"/html/body/table/tbody");
            var tableElements = new List<ReadOnlyCollection<string>>();
            if (c != null)
            {
                var nodes = c.SelectNodes("//td");
                int m = 0;
                do
                {
                    var tableRow = new List<string>();
                    tableRow.Add(nodes[m++].InnerText.Trim());
                    tableRow.Add(nodes[m++].InnerText.Trim());
                    tableRow.Add(nodes[m++].InnerText.Trim());
                    tableElements.Add(tableRow.AsReadOnly());
                } while (m < nodes.Count);

                foreach (var row in tableElements)
                {
                    Console.WriteLine("{0} \t {1} \t {2}", row[0], row[1], row[2]);
                }
            }

            Console.ReadLine();
        }

        static string getResponse(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.UserAgent = "My applicartion name";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192))
            {
                var s = reader.ReadToEnd();
                Console.WriteLine(s);
               return reader.ReadToEnd();
            }
        }
    }
}
