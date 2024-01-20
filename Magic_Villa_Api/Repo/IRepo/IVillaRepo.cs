using Magic_Villa_Api.Modeles;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo.IRepo
{
    public interface IVillaRepo : IRepo<Villa>
    {

        Task<Villa> Update(Villa item);
      
    }
}
