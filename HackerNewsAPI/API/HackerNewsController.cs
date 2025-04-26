using HackerNewsAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNews _hackerNews;
        private readonly ILogger<HackerNewsController> _logger;

        /// <summary>
        /// HackerNewsController
        /// </summary>
        /// <param name="hackerNews"></param>
        /// <param name="logger"></param>
        public HackerNewsController(IHackerNews hackerNews,
            ILogger<HackerNewsController> logger)
        {
            _hackerNews = hackerNews;
            _logger = logger;
        }

        /// <summary>
        /// Get Hacker News Stories
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                var getHackerNews = await _hackerNews.GetHackerNews();

                if (getHackerNews == null)
                    return BadRequest("Failed to Fetch Hacker news data");

                return Ok(getHackerNews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Hacker news data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
