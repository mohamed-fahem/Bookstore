
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IBookstoreRepository<Author> _authorRepository;

        // GET: Author

        public AuthorController(IBookstoreRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IActionResult> Index()
        {
            var authors = await _authorRepository.GetAll();
            return View(authors);
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // GET: Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _authorRepository.Add(author);
                    return RedirectToAction(nameof(Index));
                }
                return View(author);
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Author author)
        {
            try
            {
                if (id != author.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _authorRepository.Update(author);
                    
                    return RedirectToAction(nameof(Index));
                }
                return View(author);
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var author = _authorRepository.GetById(id).Result;
                _authorRepository.Delete(author);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
