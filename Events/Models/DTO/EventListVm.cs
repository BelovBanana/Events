using Events.Models.Domain;

namespace Events.Models.DTO
{
    public class EventListVm
    {
        public IQueryable<Event> EventList { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
    }
}
