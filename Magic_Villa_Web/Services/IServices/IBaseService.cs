using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Models;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IBaseService
    {
        //1-we simply add generic send task wich we send request 
        //2- then we add or save the response
        //(we turn it from regular response to our api response model)

        public APIResponse APIResponse { get; set; }

        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
