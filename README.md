# Crawler
A small web crawler used to collect Kurdish text over the web

It has these commands:
 - **Crawl:** used to crawl web pages and save them to a folder on disk.
 - **Extract:** used to extact kurdish text from the pages that are collected from the previous command.
 - **Normalize:** used to convert the text collected in the previous command to standard unicode text.
 - **Merge:** Used to merge the text files produced from the previous commands.
 - **WordList:** used to make a wordlist from the text file that's produced from the previous command.

## How to use

### Crawl
```
./crawler.exe crawl -seed <seed> -output <output> [-delay <delay>] [-pages <pages>]
```

#### Parameters:
- `seed`: The text file containing the **absolute** URLs of the seed pages. The more links a seed page contains, the better. The URLs in the text file should be seperated by a line break, like so:

```
https://en.wikipedia.org/wiki/Main_Page
https://en.wikipedia.org/wiki/Wikipedia:About
...
```
- `output`: The folder to save the crawled pages. The crawler will also save a `$Stats.txt` file that contains the crawling stats.
- `delay`: Number of milliseconds to wait between crawling two pages. Default value is `1000`
- `pages`: Maximum number of pages to crawl per **each** seed page. Default value is `250`

#### Examples:
```
./crawler.exe crawl -seed ./seed.txt -output ./Data
./crawler.exe crawl -seed ./seed.txt -output D:\CrawledPages\ -delay 250 -pages 1000
```
