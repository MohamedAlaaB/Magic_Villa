using Magic_Villa_Api.Data;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo
{
    public class VNumRepo : Repo<VillaNumber>, IVillaNumber
    {
        private readonly applicationDbContext _context;
        public VNumRepo(applicationDbContext context):base(context) 
        {
            _context = context;
        }
      
        public async Task<VillaNumber> Update(VillaNumber item)
        {
            _context.VNum.Update(item);
           await _context.SaveChangesAsync();
            return item;
        }
    }
}
