using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace CoroutineFoundation
{
    public class WaitForParseHtml : IWait
    {
        private int m_WaitTasks = 0;
        private List<string> m_Urls = new List<string>();
        private string m_Condition = "";
        private DateTime m_TempTime;

        public WaitForParseHtml(int tasks, List<string> urls, string condition)
        {
            m_WaitTasks = tasks;
            m_Urls = urls;
            m_Condition = condition;
            m_TempTime = DateTime.Now;
        }

        bool IWait.Tick()
        {
            GetHtmlAsync(m_Urls[m_WaitTasks - 1]);
            m_WaitTasks--;
            return m_WaitTasks <= 0;
        }

        public void GetHtmlAsync(string url)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += GetHtmlAsync_OpenReadCompleted;
            webClient.OpenReadAsync(new Uri(url));
        }

        private async void GetHtmlAsync_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (StreamReader streamReader = new StreamReader(e.Result, Encoding.Default))
            {
                string html = await streamReader.ReadToEndAsync();
                ParseHtml(html, m_Condition);
            }
        }

        public void ParseHtml(string html, string condition)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes(condition);
            foreach (HtmlNode linkNode in linkNodes)
            {
                Console.WriteLine("协程" + linkNode.InnerText);
            }
            Console.WriteLine("耗时：{0}秒", (DateTime.Now - m_TempTime).TotalSeconds);
        }
    }
}
