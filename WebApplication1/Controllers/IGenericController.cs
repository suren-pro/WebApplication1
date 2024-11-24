using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public interface IGenericController<T> where T : class
    {
        Task<IActionResult> Get();
        Task<IActionResult> Get(int page,int count);
        Task<IActionResult> Create(T t);
        Task<IActionResult> Update(T t);
        Task<IActionResult> Delete(int id);
    }
}
