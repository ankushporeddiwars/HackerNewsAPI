using HackerNewsAPI.Application.Models;
using HackerNewsAPI.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        public HackerNewsService(IMemoryCache memoryCache, HttpClient httpClient,
            ILogger<HackerNewsService> logger)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Get Hacker News Stories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HackerStoryModel>> GetHackerNewsStories()
        {
            var result = new List<HackerStoryModel>();
            try
            {
                if (_memoryCache.TryGetValue(CacheKey, out result))
                {
                    return result;
                }

                var storyIds = await _httpClient.GetFromJsonAsync<List<long>>($"{BaseUrl}/newstories.json");

                if (storyIds != null && storyIds.Count > 0)
                {
                    var ids = storyIds.OrderByDescending(x => x).Take(200);

                    var task = ids.Select(async id =>
                    {
                        var story = await _httpClient.GetFromJsonAsync<HackerStoryModel>($"{BaseUrl}/item/{id}.json");
                        return story;
                    });

                    var stories = await Task.WhenAll(task);

                    result = stories.Where(story => story != null).ToList();

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    };

                    _memoryCache.Set(CacheKey, result, cacheOptions); ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Hacker news data");
            }

            return result;
        }


        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";
        /// <summary>
        /// IMemoryCache
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// HttpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// ILogger
        /// </summary>
        private readonly ILogger<HackerNewsService> _logger;

        /// <summary>
        /// Cache key for storing Hacker News stories
        /// </summary>
        private const string CacheKey = "HackerNewsStories";

    }
}
