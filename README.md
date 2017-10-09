# Crawler
A small web crawler used to collect Kurdish text over the web

It has these commands:
 - [X] **Crawl:** used to crawl web pages and extact kurdish text from them and save them to a folder on disk.
 - [ ] **Normalize:** used to convert the text collected in the previous command to standard unicode text.
 - [ ] **Merge:** Used to merge the text files produced from the previous commands.
 - [ ] **WordList:** used to make a wordlist from the text file that's produced from the previous command.

## How to use

### Crawl
```
./crawler.exe crawl -url <url> -output <output> [-delay <delay>] [-pages <pages>]
```

#### Parameters:
- `url`: The **absolute** URL for the site you want to crawl.
- `output`: The folder to save the crawled pages. The crawler will also save a `$Stats.txt` file that contains the crawling stats.
- `delay`: Number of milliseconds to wait between crawling two pages. Default value is `1000`
- `pages`: Maximum number of pages to crawl. Default value is `250`

#### Examples:
```
./crawler.exe crawl -url https://ckb.wikipedia.org -output ./Data
./crawler.exe crawl -url https://www.google.iq/ -output D:/CrawledPages/ -delay 250 -pages 1000
```
