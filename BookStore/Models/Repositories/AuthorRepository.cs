using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookstoreRepository<Author>
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> Add(Author entity)
        {
            await _context.Authors.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public  Author Delete(Author entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetById(int id)
        {
            return await _context.Authors.SingleOrDefaultAsync(x => x.Id==id);
        }

        public Author Update(Author entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;  
        }

    }
}
