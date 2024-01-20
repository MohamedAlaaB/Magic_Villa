using Magic_Villa_Api.Modeles;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo.IRepo
{
    public interface IRepo <T> where T : class
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null ,string? includeprops = null ,int pagesize = 0 ,int pagenumber=1);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeprops = null);
        Task Remove(T item);
        
        Task Add(T item);
        Task Save();
    }
}
