using HackerNewsAPI.API;
using HackerNewsAPI.Application.Models;
using HackerNewsAPI.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HackerNewsAPI.UnitTest
{
    public class HackerNewsControllerTest
    {
        private readonly Mock<IHackerNews> _hackerNews;
        private readonly HackerNewsController hackerNewsController;
        public HackerNewsControllerTest()
        {
            _hackerNews = new Mock<IHackerNews>();
            hackerNewsController =new HackerNewsController(_hackerNews.Object, new Mock<ILogger<HackerNewsController>>().Object);
        }

        [Fact]
        public async Task GetHackerNewsStories_ShouldReturnOk()
        {
            _hackerNews.Setup(x => x.GetHackerNews()).ReturnsAsync(GetHackerStories());

            var result = await hackerNewsController.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<HackerStoryModel>>(okResult.Value);
            Assert.Equal(3, ((List<HackerStoryModel>)okResult.Value).Count);
        }

        private List<HackerStoryModel> GetHackerStories()
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
