using Events.Models.Domain;
using Events.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Events.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genService;
        public EventController(IGenreService genService, IEventService EventService, IFileService fileService)
        {
            _eventService = EventService;
            _fileService = fileService;
            _genService = genService;
        }

        //Отображаем форму добавленого мероприятия
        public IActionResult Add() 
        {
            var model = new Event();
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]

        //Добавляем новое мероприятие
        public IActionResult Add(Event model)
        {
            var selectedGenres = _eventService.GetGenreByEventId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "Файл не удалось сохранить";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.EventImage = imageName;
            }
            var result = _eventService.Update(model);
            if (result)
            {
                TempData["msg"] = "Успешно добавлен";
                return RedirectToAction(nameof(EventList));
            }
            else
            {
                TempData["msg"] = "Не удалось сохранить";
                return View(model);
            }
        }

        //Отображение формы редактирования
        public IActionResult Edit(int id)
        {
            var model = _eventService.GetById(id);
            var selectedGenres = _eventService.GetGenreByEventId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]

        //Редактирование мероприятий c существующей инфой
        public IActionResult Edit(Event model)
        {
            var selectedGenres = _eventService.GetGenreByEventId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.EventImage = imageName;
            }
            var result = _eventService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(EventList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        [HttpPost]

        //Редактирование мероприятий (Жанры)
        public IActionResult Update(Event model)
        {
            //model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.EventImage = imageName;
            }
            var result = _eventService.Update(model);
            if (result)
            {
                TempData["msg"] = "Успешно добавлен";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Не удалось сохранить";
                return View(model);
            }
        }

        //Просмотр всего списка мероприятий
        public IActionResult EventList()
        {
            var data = this._eventService.List();
            return View(data);
        }

        //Удаление
        public IActionResult Delete(int id)
        {
            var result = _eventService.Delete(id);
            return RedirectToAction(nameof(EventList));
        }
    }
}
