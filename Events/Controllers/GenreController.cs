using Events.Models.Domain;
using Events.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            this._genreService = genreService;
        }
        
        //Отображаем страницу для нового жанра
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        //Заполнение форму нового жанра
        public IActionResult Add(Genre model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var result = _genreService.Add(model);
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

        //Редактирование жанра (отображение)
        public IActionResult Edit(int id)
        {
            var data= _genreService.GetById(id);
            return View(data);
        }

        [HttpPost]
        //Редактирование жанра
        public IActionResult Update(Genre model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = _genreService.Update(model);
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

        //Смотрим на все жанры
        public IActionResult GenreList()
        {
            var data = this._genreService.List().ToList();
            return View(data);
        }

        //Ну тут короче удаление
        public IActionResult Delete(int id)
        {
            var result = _genreService.Delete(id);
            return RedirectToAction(nameof(GenreList));
        }
    }
}
