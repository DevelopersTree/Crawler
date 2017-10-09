using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Uri _seedUrl;
        private int _delay;
        private int _maxPages;
        private List<WebPage> _webPages;
        private string _statsFilePath;
        private const string StatsFileName = "$Stats.txt";
        public Crawler(string[] args)
        {
            _seedUrl = new Uri(ParameterHelper.GetParameter(args, "-url", "absolute url"));
            _savePath = ParameterHelper.GetParameter(args, "-output", $" output path");
            _delay = ParameterHelper.GetIntegerParameter(args, "-delay", 1000);
            _maxPages = ParameterHelper.GetIntegerParameter(args, "-pages", 250);
            _statsFilePath = ParameterHelper.GetPath(_savePath, StatsFileName);
        }

        public string Crawl(List<WebPage> webPages)
        {
            if (File.Exists(_statsFilePath) && webPages.Count == 0)
                File.Delete(_statsFilePath);

            _webPages = webPages ?? new List<WebPage>();
           
            IWebCrawler crawler;
           
            crawler = GetDefaultWebCrawler(_maxPages, _delay);

            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            CrawlResult result = crawler.Crawl(_seedUrl);

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
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var contents = Extractor.Extract(e.CrawledPage.Content.Text);

            var page = new WebPage
            {
                FileName = ParameterHelper.GetPath(_savePath, (_webPages.Count + 1).ToString() + ".txt"),
                Url = e.CrawledPage.Uri.AbsoluteUri,
                NumberOfWords = contents.Split(' ').Length
            };

            _webPages.Add(page);

            IOHelper.SaveFile(page.FileName, contents);

            SaveStats(page);

            stopWatch.Stop();
            Console.WriteLine("Pages crowled: " + _webPages.Count);
            Console.WriteLine($"Page Crawled: {page.Url}, Number Of Words: {page.NumberOfWords:n0}. Processed in {stopWatch.ElapsedMilliseconds:n0} ms");
        }

        public void SaveStats(WebPage page)
        {
            var content = $"{page.Url},{page.FileName},{page.NumberOfWords}{Environment.NewLine}";

            if (File.Exists(_statsFilePath))
                File.AppendAllText(_statsFilePath, content);
            else
                IOHelper.SaveFile(_statsFilePath, content);
        }

    }
}
