using Events.Models.Domain;
using Events.Models.DTO;

namespace Events.Repositories.Abstract
{
    public interface IEventService
    {
        bool Add(Event model);
        bool Update(Event model);
        Event GetById(int id);
        bool Delete(int id);
        EventListVm List(string term ="", bool paging = false, int currentPage = 0);
        List<int> GetGenreByEventId(int eventId);
    }
}
