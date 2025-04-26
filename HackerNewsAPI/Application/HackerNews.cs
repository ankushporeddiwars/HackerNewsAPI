using HackerNewsAPI.Application.Models;
using HackerNewsAPI.Contracts;

namespace HackerNewsAPI.Application
{
    public class HackerNews : IHackerNews
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<HackerNews> _logger;
        public HackerNews(IHackerNewsService hackerNewsService,
            ILogger<HackerNews> logger)
        {
            _hackerNewsService = hackerNewsService;
        }
        public async Task<IEnumerable<HackerStoryModel>> GetHackerNews()
        {
            var result = new List<HackerStoryModel>();
            try
            {
                var hackerStories = await _hackerNewsService.GetHackerNewsStories();

                result = hackerStories.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Hacker news data");
            }
            return result;
        }
    }
}
