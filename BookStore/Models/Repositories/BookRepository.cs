using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookstoreRepository<Book>
    {
        
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> Add(Book entity)
        {
            await _context.Books.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public Book Delete(Book entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.Include(x=>x.Author).ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books.Include(x=>x.Author).SingleOrDefaultAsync(b=>b.Id == id);
        }

        public Book Update(Book entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

    }
}
