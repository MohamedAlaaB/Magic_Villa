using Magic_Villa_Api.Data;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo
{
    public class Repo<T>:IRepo<T> where T : class
    {
        private readonly applicationDbContext _context;
        private DbSet<T> dbSet;
        public Repo(applicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
           
        }
        public async Task Add(T item)
        {
            await _context.AddAsync(item);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeprops = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked) { query.AsNoTracking(); }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if(includeprops != null)
            {
                foreach (var item in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string? includeprops = null, int pagesize = 0, int pagenumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pagenumber >0)
            {
                if (pagesize>100)
                {
                    pagesize = 100;
                }
                query = query.Skip((pagenumber - 1) * pagesize).Take(pagesize);
            }
            if (includeprops != null)
            {
                foreach (var item in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
           
            return await query.ToListAsync();
        }

        public async Task Remove(T item)
        {
            dbSet.Remove(item);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}
