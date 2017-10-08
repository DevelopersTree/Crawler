using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class Crawler
    {
        private const char IgnoreCharacter = '\0';

        private string _savePath = null;
        private Uri _uriToCrawl;
        private int _delay;
        private int _maxPages;
        private List<WebPage> _webPages;
        private const string StatsFileName = "$Stats.txt";
        public Crawler(string[] args)
        {
            _uriToCrawl = new Uri(ParameterHelper.GetParameter(args, "-url", "absolute url"));
            _savePath = ParameterHelper.GetParameter(args, "-output", $" output path");
            _delay = ParameterHelper.GetIntegerParameter(args, "-delay", 1000);
            _maxPages = ParameterHelper.GetIntegerParameter(args, "-pages", 250);
        }

        public string Crawl(List<WebPage> webPages)
        {
            _webPages = webPages ?? new List<WebPage>();
            IWebCrawler crawler;

            crawler = GetDefaultWebCrawler(_maxPages, _delay);

            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            CrawlResult result = crawler.Crawl(_uriToCrawl);

            return _savePath;
        }

        private IWebCrawler GetDefaultWebCrawler(int maxPagesToCrawl, int delayInMilliseconds)
        {
            var config = new CrawlConfiguration();
            config.CrawlTimeoutSeconds = 0;
            config.DownloadableContentTypes = "text/html, text/plain";
            config.IsExternalPageCrawlingEnabled = false;
            config.IsExternalPageLinksCrawlingEnabled = false;
            config.IsRespectRobotsDotTextEnabled = true;
            config.IsUriRecrawlingEnabled = false;
            config.IsHttpRequestAutoRedirectsEnabled = true;
            config.UserAgentString = "DevTree Crawler";
            config.MaxConcurrentThreads = 10;
            config.MaxPagesToCrawl = maxPagesToCrawl;
            config.MinCrawlDelayPerDomainMilliSeconds = delayInMilliseconds;

            return new PoliteWebCrawler(config);
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var contents = e.CrawledPage.Content.Text;

            var page = new WebPage
            {
                FileName = ParameterHelper.GetPath(_savePath, (_webPages.Count + 1).ToString() + ".txt"),
                Url = e.CrawledPage.Uri.AbsoluteUri
            };

            _webPages.Add(page);

            IOHelper.SaveFile(page.FileName, contents);

            Console.WriteLine("Pages crowled: " + _webPages.Count);
            Console.WriteLine($"Page Crawled: {page.Url}, Saved to: {page.FileName}.");
        }

        public void SaveStats()
        {
            var statistics = _webPages.Select(w => $"{w.Url}, {w.FileName}").ToArray();
            IOHelper.SaveFile(ParameterHelper.GetPath(_savePath, StatsFileName), statistics);
        }

    }
}
