using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawl
{
    internal class WebCrawl
    {
        private string baseAddress = "https://github.com/";
        private string codeWareHouse = "ThinkGeo";

        private string BaseUrl
        {
            get
            {
                return baseAddress + codeWareHouse;
            }
        }

        private string result = "";
        private Collection<string> selectKeys = new Collection<string>();

        private static void Main(string[] args)
        {
            WebCrawl crawler = new WebCrawl();
            int number = int.Parse(ConfigurationManager.AppSettings["selectKeysCount"]);
            Console.WriteLine(number);
            for (int i = 0; i < number; i++)
            {
                String selectKey = ConfigurationManager.AppSettings["selectKey" + i];
                Console.WriteLine(selectKey);
                if (selectKey.Equals("all"))
                {
                    crawler.selectKeys.Clear();
                    selectKey = @"[\w]+";
                    crawler.selectKeys.Add(selectKey);
                    break;
                }
                crawler.selectKeys.Add(selectKey);
            }
            crawler.result = crawler.CrawlByKeys();
            crawler.WriteResult();
        }

        public string CrawlByKeys()
        {
            Collection<string> allLinks = GetAllPageLinks();
            Collection<string> keys = new Collection<string>(selectKeys);
            foreach (string link in allLinks)
            {
                if (keys.Count > 0)
                {
                    string html = visitUrl(link);
                    for (var i = 0; i < keys.Count; i++)
                    {
                        Regex regexKey = new Regex(@"a[\s]+href=(?:""|')/" + codeWareHouse + @"/([^<>""']*)" + keys[i] + @"([^<>""']*)(?:""|')", RegexOptions.IgnoreCase);
                        MatchCollection matchResults = regexKey.Matches(html);
                        foreach (var str in matchResults)
                        {
                            var start = str.ToString().IndexOf(codeWareHouse);
                            start += codeWareHouse.Length + 1; //加上ThinkGeo后面的/
                            var r = str.ToString().Substring(start, str.ToString().Length - start - 1);  //去掉后面的引号
                            Console.WriteLine(r);
                            if (r.Equals(keys[i]))
                            {
                                keys.RemoveAt(i);
                                break;
                            }
                            result += r + "\r\n";
                        }
                    }
                }
            }
            return result;
        }

        public void WriteResult()
        {
            String path = ConfigurationManager.AppSettings["resultPath"];
            FileStream fs = new FileStream(@path, FileMode.Create, FileAccess.Write);
            fs.Position = fs.Length;
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(result);
            sw.Close();
        }

        public Collection<string> GetAllPageLinks()
        {
            string html = visitUrl(BaseUrl);
            string pagination = "";
            int paginationIndex = html.IndexOf(@"<div class=""pagination"">");
            string restStr = html.Substring(paginationIndex);
            int endIndex = restStr.IndexOf("</div>");
            pagination = restStr.Substring(0, endIndex);
            Regex regexLink = new Regex(@"<[^<>]+>[^<>\s]+</[^<>]+>");
            MatchCollection matchLinks = regexLink.Matches(pagination.ToString());
            Collection<string> links = new Collection<string>();
            int pageCount = matchLinks.Count - 2; //去掉这个pagination中的previous和next模块，所以是减2
            for (var i = 1; i <= pageCount; i++)
            {
                links.Add(baseAddress + codeWareHouse + "?page=" + i);
            }
            return links;
        }

        public string visitUrl(string url)
        {
            WebRequest request = WebRequest.Create(url.Trim());
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(resStream, Encoding.Default);
            StringBuilder stringBuilder = new StringBuilder();
            while ((url = streamReader.ReadLine()) != null)
            {
                stringBuilder.Append(url);
            }
            return stringBuilder.ToString();
        }
    }
}