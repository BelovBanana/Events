using System.ComponentModel.DataAnnotations;

namespace Events.Models.Domain
{
    public class EventsGenre
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int GenreId { get; set; }
    }
}
