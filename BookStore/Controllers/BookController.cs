using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book>  _bookRepository;
        private readonly IBookstoreRepository<Author> _authorRepository;
        private readonly IHostingEnvironment  _hosting;
        public BookController(IBookstoreRepository<Book> bookRepository, IBookstoreRepository<Author> authorRepository, IHostingEnvironment hosting)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _hosting = hosting;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAll();
            return View(books);
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookAuthorViewModel model)
        {
            try
            {
                string filename = UploadFile(model.File) ?? string.Empty;
                
                if (model.AuthorId == -1)
                {
                    ViewBag.Message = "Please select an author from list!";
                    var vmodel = new BookAuthorViewModel
                    {
                        Authors = FillSelectList()
                    };
                    return View(vmodel);
                }
                var book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = _authorRepository.GetById(model.AuthorId).Result,
                    ImageUrl = filename

                };

                await _bookRepository.Add(book);
                return RedirectToAction(nameof(Index));




                
               

                
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            var authorid = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var authors = _authorRepository.GetAll().Result.ToList();
            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorid,
                Authors = authors,
                ImageUrl = book.ImageUrl
                
            };
            

            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( BookAuthorViewModel model)
        {
            try
            {

                string filename = UploadFile(model.File,model.ImageUrl);
               
                var book = new Book
                {
                    Id=model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = _authorRepository.GetById(model.AuthorId).Result,
                    ImageUrl = filename
                    
                };
                _bookRepository.Update(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id,BookAuthorViewModel model)
        {
            try
            {
                var book = _bookRepository.GetById(id).Result;
                _bookRepository.Delete(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var author = _authorRepository.GetAll().Result.ToList();
            author.Insert(0, new Author { Id = -1, FullName = "--- Please select an author ---" });
            return author;
        }


        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(_hosting.WebRootPath, "uploads");               
                string fullpath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullpath, FileMode.Create));
                return file.FileName;
            }

            return null;
        }

        string UploadFile(IFormFile file,string imageUrl)
        {
            if (file != null)
            {
                string upload = Path.Combine(_hosting.WebRootPath, "uploads");
                string newpath = Path.Combine(upload, file.FileName);

                // Delete the old file
                string oldpath = Path.Combine(upload, imageUrl);
                if (oldpath != newpath)
                {
                    System.IO.File.Delete(oldpath);

                    // Save new file
                    file.CopyTo(new FileStream(newpath, FileMode.Create));
                }
                return file.FileName;

            }

            return imageUrl;
        }
    }
}
