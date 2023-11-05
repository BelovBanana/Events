using Events.Models.Domain;
using Events.Models.DTO;
using Events.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Events.Repositories.Implementation
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext ctx;

        public EventService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Event model)
        {
            try
            {
                ctx.Event.Add(model);
                ctx.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var eventGenre = new EventsGenre
                    {
                        EventId = model.Id,
                        GenreId = genreId
                    };
                    ctx.EventsGenre.Add(eventGenre);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                var EventGenres = ctx.EventsGenre.Where(e => e.EventId == data.Id);
                foreach (var eventGenre in EventGenres)
                {
                    ctx.EventsGenre.Remove(eventGenre);
                }
                ctx.Event.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Event GetById(int id)
        {
            return ctx.Event.Find(id);
            
        }

        public EventListVm List(string term="", bool paging= false, int currentPage=0 )
        {
            var data = new EventListVm();
            var list = ctx.Event.ToList();
            if(!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a=>a.Title.ToLower().StartsWith(term)).ToList();
            }
            if (!paging)
            {
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage-1)*pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }
            foreach (var events in list)
            {
                var genres = (from genre in ctx.Genre
                              join mg in ctx.EventsGenre
                              on genre.Id equals mg.GenreId
                              where mg.EventId == events.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                events.GenreNames = genreNames;
            }

            data.EventList = list.AsQueryable();
            return data;
        }

        public bool Update(Event model)
        {
            try
            {
                var genresToDeleted = ctx.EventsGenre.Where(a => a.EventId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach(var eventGenre in genresToDeleted)
                {
                    ctx.EventsGenre.Remove(eventGenre);
                }
                foreach (int genId in model.Genres)
                {
                    var eventGenre = ctx.EventsGenre.FirstOrDefault(a => a.EventId == model.Id && a.GenreId == genId);
                    if (eventGenre == null)
                    {
                        eventGenre = new EventsGenre { GenreId = genId, EventId = model.Id };
                        ctx.EventsGenre.Add(eventGenre);
                    }
                }
                ctx.Event.Update(model);
                //мы должны добавить эти идентификаторы жанров в таблицу Eventgenre
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<int> GetGenreByEventId(int eventId)
        {
            var genreIds = ctx.EventsGenre.Where(a=>a.EventId== eventId).Select(a=>a.GenreId).ToList();
            return genreIds;
        }
    }
}
