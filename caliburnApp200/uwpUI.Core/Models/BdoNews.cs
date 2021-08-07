using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace uwpUI.Core.Models
{
    public partial class NewsResult
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("hashtag")]
        public string Hashtag { get; set; }

        [JsonProperty("category")]
        public long Category { get; set; }

        [JsonProperty("list")]
        public List<NewsItem> List { get; set; }

        [JsonProperty("searchType")]
        public string SearchType { get; set; }

        [JsonProperty("searchText")]
        public string SearchText { get; set; }

        [JsonProperty("pageNo")]
        public long PageNo { get; set; }

        [JsonProperty("pageSize")]
        public long PageSize { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }
    }

    public partial class NewsItem
    {
        [JsonProperty("boardNo")]
        public int BoardNo { get; set; }

        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subTitle")]
        public string SubTitle { get; set; }

        [JsonProperty("imageURL")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("thumbnailURL")]
        public Uri ThumbnailUrl { get; set; }

        [JsonProperty("regDate")]
        public string RegDate { get; set; }

        [JsonProperty("hashTags")]
        public string HashTags { get; set; }
        public string NavDetailUri => string.IsNullOrEmpty(DetailUrl) ? $"http:{DetailUrl}": string.Empty;
        public string DetailUrl { get; set; }
    }
}

