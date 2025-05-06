
namespace InventoryMangmentSystem.Api.MiddleWare
{
    public class GlobalErrorHandle : IMiddleware
    {
         Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
