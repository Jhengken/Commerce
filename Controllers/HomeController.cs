using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration Config;

		public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
			Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
		}

		private ICommerce GetPayType(string option)
		{
			switch (option)
			{
				case "newbPay":
					return new NewebpayService();

				default: throw new ArgumentException("No Such option");
			}
		}

		public IActionResult Index()
        {
            // 產生測試資訊
            ViewData["MerchantID"] = Config.GetSection("MerchantID").Value;
            ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
            ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限       
            return View();
        }

		public IActionResult SendToNewebPay(SendToNewebPayIn inModel)
		{
			var service = GetPayType(inModel.PayOption);

			return Json(service.GetCallBack(inModel));
		}


		/// <summary>
		/// 支付完成返回網址
		/// </summary>
		/// <returns></returns>
		public IActionResult CallbackReturn(string option)
		{
			var service = GetPayType(option);
			var result = service.GetCallbackResult(Request.Form);
			ViewData["ReceiveObj"] = result.ReceiveObj;
			ViewData["TradeInfo"] = result.TradeInfo;

			return View();
		}

		/// <summary>
		/// 商店取號網址
		/// </summary>
		/// <returns></returns>
		public IActionResult CallbackCustomer(string option)
		{
			var service = GetPayType(option);
			var result = service.GetCallbackResult(Request.Form);
			ViewData["ReceiveObj"] = result.ReceiveObj;
			ViewData["TradeInfo"] = result.TradeInfo;
			return View();
		}

		/// <summary>
		/// 支付通知網址
		/// </summary>
		/// <returns></returns>
		public IActionResult CallbackNotify(string option)
		{
			var service = GetPayType(option);
			var result = service.GetCallbackResult(Request.Form);
			ViewData["ReceiveObj"] = result.ReceiveObj;
			ViewData["TradeInfo"] = result.TradeInfo;
			return View();
		}

	}
}