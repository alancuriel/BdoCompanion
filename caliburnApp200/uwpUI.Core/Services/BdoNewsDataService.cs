using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using uwpUI.Core.Helpers;
using uwpUI.Core.Models;
using HtmlAgilityPack;
using System.Linq;

namespace uwpUI.Core.Services
{
    public static class BdoNewsDataService
    {
        private static HttpClient Client { get; set; }
        private static HtmlDocument HtmlDocument { get; set; }

        public static async Task InitializeAsync()
        {
            await Task.CompletedTask;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
            Client.DefaultRequestHeaders
                .TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            HtmlDocument = new HtmlDocument();
        }

        public static async Task<IEnumerable<NewsItem>> GetXboxNews()
        {
            //byte[] bytes = Encoding.UTF8.GetBytes("pageNo=1&category=0&hashtag=xbox&searchText=&searchType=\r\n\r\n");
            //var formContent = new ByteArrayContent(bytes);
            //formContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
            //{
            //    CharSet = "UTF-8"
            //};


            //var response = await Client.
            //    PostAsync("https://www.console.playblackdesert.com/xbox/News/List", formContent);

            //if (!response.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            //var json = await response.Content.ReadAsStringAsync();

            //var jsonObject = await Json.ToObjectAsync<NewsResult>(json);

            //foreach (var item in jsonObject.Result.List)
            //{
            //    item.DetailUrl = $"https://www.console.playblackdesert.com/xbox/News/Detail?boardNo={item.BoardNo}&category=0";
            //}

            //return jsonObject.Result.List;
            return await GetPcNews(isConsole: true);
        }

        public static async Task<IEnumerable<NewsItem>> GetPcNews(bool isSea = false, bool isConsole = false)
        {
            string url;
            if (isSea)
            {
                url = "https://www.sea.playblackdesert.com/News/Notice";
            }
            else
            {
                url = "https://www.naeu.playblackdesert.com/en-US/News/Notice";
            }

            if (isConsole)
            {
                url = "https://www.console.playblackdesert.com/News/Notice";
            }

            var response = await Client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var html = await response.Content.ReadAsStringAsync();

            return isSea ? ScrapePcNews(ref html, true) : ScrapePcNews(ref html);
        }

        private static IEnumerable<NewsItem> ScrapePcNews(ref string html, bool isSea = false)
        {
            HtmlDocument.LoadHtml(html);
            var newsItems = new List<NewsItem>();
            string list_class;
            string subtitle_class;
            string date_class;

            if (true)
            {
                list_class = "thumb_nail_list";
                subtitle_class = "desc";
                date_class = "date";
            }
            else
            {
                list_class = "list_news";
                subtitle_class = "desc_news";
                date_class = "txt_time";
            }




            var list_newsNodes = HtmlDocument.DocumentNode.Descendants("ul")
                .Where(n => n.GetAttributeValue("class", "") == list_class).FirstOrDefault().Descendants("a");

            foreach (var newsNode in list_newsNodes)
            {
                var newsItem = new NewsItem();

                newsItem.DetailUrl = isSea ? newsNode.GetAttributeValue("href", string.Empty) 
                                     : $"https://www.blackdesertonline.com{newsNode.GetAttributeValue("href", string.Empty)}";
                  

                var imgNode = newsNode.Descendants("img").FirstOrDefault();
                if (imgNode != null)
                {
                    newsItem.ImageUrl = new Uri(imgNode.GetAttributeValue("src", string.Empty));
                }

                var titleNode = newsNode.Descendants("strong").FirstOrDefault();
                newsItem.Title = titleNode?.GetDirectInnerText().Trim(new char[] { ' ', '\n', '\r', '\t' });

                var spanNodes = newsNode.Descendants("span");

                var subTitleNode = spanNodes.Where(n => n.GetAttributeValue("class", "") == subtitle_class)
                    .FirstOrDefault();
                newsItem.SubTitle = subTitleNode?.InnerText.Trim(new char[] { ' ', '\n', '\r', '\t' });

                var dateNode = spanNodes.Where(n => n.GetAttributeValue("class", "") == date_class).FirstOrDefault();
                newsItem.RegDate = dateNode?.InnerText;



                newsItems.Add(newsItem);
            }

            return newsItems;
        }
    }
}
