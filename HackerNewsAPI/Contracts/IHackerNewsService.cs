using HackerNewsAPI.Application.Models;

namespace HackerNewsAPI.Contracts
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerStoryModel>> GetHackerNewsStories();
    }
}