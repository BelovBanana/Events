using Events.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;
        public HomeController(IEventService eventService)
        {
            _eventService = eventService;
        }
        //Отображает домашнюю страницу
        public IActionResult Index(string term="", int currentPage = 1)
        {
            var events = _eventService.List(term, true, currentPage);
            return View(events);
        }

        //Отображает инфу со страницы О нас
        public IActionResult About()
        {
            return View();
        }

        //Детальная инфа о конкретном мероприятии
        public IActionResult EventDetail(int eventId)
        {
            var events = _eventService.GetById(eventId);
            return View();
        }
    }
}
