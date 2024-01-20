using Magic_Villa_Api.Data;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo
{
    public class VillaRepo : Repo<Villa>,IVillaRepo
    {
        private readonly applicationDbContext _context;
        public VillaRepo(applicationDbContext context):base(context) 
        {
            _context = context;
        }
      
        public async Task<Villa> Update(Villa item)
        {
            _context.villas.Update(item);
           await _context.SaveChangesAsync();
            return item;
        }
    }
}
