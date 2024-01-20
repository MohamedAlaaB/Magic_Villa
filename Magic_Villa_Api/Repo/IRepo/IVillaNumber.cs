using Magic_Villa_Api.Modeles;
using System.Linq.Expressions;

namespace Magic_Villa_Api.Repo.IRepo
{
    public interface IVillaNumber: IRepo<VillaNumber>
    {

        Task<VillaNumber> Update(VillaNumber item);
      
    }
}
