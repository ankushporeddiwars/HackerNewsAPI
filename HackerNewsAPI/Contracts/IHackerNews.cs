using HackerNewsAPI.Application.Models;

namespace HackerNewsAPI.Contracts
{
    public interface IHackerNews
    {
        Task<IEnumerable<HackerStoryModel>> GetHackerNews();
    }
}
