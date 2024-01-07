using BooksCatalog;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace BooksCatalogWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookCatalogController : ControllerBase
    {
        private BooksComponent _bookComponents;

        [ActivatorUtilitiesConstructor]
        public BookCatalogController(IConfiguration configuration)
        {
           _bookComponents = new BooksComponent(configuration);
        }

        /// <summary>
        /// Добавление книги в каталог
        ///<param name="title">Название</param>
        ///<param name="author">Автор</param>
        ///<param name="isbn">ISBN</param>
        ///<param name="annotaion">Аннотация</param>
        ///<param name="genre">Жанр</param>
        ///<param name="date">Дата публикации</param>
        /// </summary>
        [HttpPost("Add book")]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult AddBook(string title, string author, string isbn, string annotaion, DateTime date, string genre)
        {
            var book = new Book(title, author, isbn, annotaion, date, genre);
            _bookComponents.AddBook(book);
            return Ok();
        }


        /// <summary>
        /// Поиск книг
        /// </summary>
        /// <param name="category">Поиск по</param>
        /// <param name="keyWords">Параметры запроса</param>
        /// <returns></returns>
        [HttpGet("FindBooks")]
        public IActionResult FindBooks(Category category, string keyWords)
        {
            IEnumerable<Book> books = _bookComponents.FindBooksByCategory(category, keyWords);
            return Ok(books);
        }

        /// <summary>
        ///Список всех книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllBooks")]
        public IActionResult GetAllBooks()
        {
            IEnumerable<Book> books = _bookComponents.BookCatalog.Books;
            return Ok(books);
        }

    }
}
