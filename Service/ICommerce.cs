using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface ICommerce
    {
        string GetCallBack(SendToNewebPayIn inModel);

        Result GetCallbackResult(IFormCollection form);
    }
}
