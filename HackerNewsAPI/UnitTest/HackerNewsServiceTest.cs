using HackerNewsAPI.Application.Models;
using HackerNewsAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace HackerNewsAPI.UnitTest
{
    public class HackerNewsServiceTest
    {
        private readonly Mock<IMemoryCache> _memoryCache;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private readonly Mock<ILogger<HackerNewsService>> _logger;
        private HackerNewsService _hackerNewsService;

        public HackerNewsServiceTest()
        {
            _memoryCache = new Mock<IMemoryCache>();
            _handlerMock = new Mock<HttpMessageHandler>();
            _logger = new Mock<ILogger<HackerNewsService>>();
        }

        [Fact]
        public async Task GetHackerNewsStories_ShouldReturnStoriesFromCache()
        {
            object cachedStories = GetCachedStories();
            _memoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStories)).Returns(true);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com/")
            };

            _hackerNewsService = new HackerNewsService(_memoryCache.Object, _httpClient, _logger.Object);

            var result = await _hackerNewsService.GetHackerNewsStories();
            Assert.NotNull(result);
            Assert.IsType<List<HackerStoryModel>>(result);
        }
        
        private List<HackerStoryModel> GetCachedStories()
        {
            return new List<HackerStoryModel>
            {
                new HackerStoryModel
                {
                    By = "Test User",
                    Descendants = 10,
                    Id = 1,
                    Score = 100,
                    Time = 1234567890,
                    Type = "story",
                    Title = "Test Title",
                    Url = "https://test1.com"
                },
                new HackerStoryModel
                {
                    By = "Test User",
                    Descendants = 11,
                    Id = 1,
                    Score = 100,
                    Time = 1234567890,
                    Type = "adult story",
                    Title = "Test Title",
                    Url = "https://test2.com"
                },
                new HackerStoryModel
                {
                    By = "Test User",
                    Descendants = 12,
                    Id = 1,
                    Score = 100,
                    Time = 1234567890,
                    Type = "kids story",
                    Title = "Test Title",
                    Url = "https://test3.com"
                }
            };

        }
    }
}
