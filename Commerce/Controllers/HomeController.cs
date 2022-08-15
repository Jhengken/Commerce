using Commerce.Models;
using Commerce.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Commerce.Controllers
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
				case "ECPay":
					return new ECPayService();

				default: throw new ArgumentException("No Such option");
			}
		}

		public IActionResult Index()
		{
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
		public HttpResponseMessage CallbackNotify(string option)
		{
			var service = GetPayType(option);
			var result = service.GetCallbackResult(Request.Form);

			//TODO 支付成功後 可做後續訂單處理

			return ResponseOK();
		}


		/// <summary>
		/// 回傳給 綠界 失敗
		/// </summary>
		/// <returns></returns>
		private HttpResponseMessage ResponseError()
		{
			var response = new HttpResponseMessage();
			response.Content = new StringContent("0|Error");
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}

		/// <summary>
		/// 回傳給 綠界 成功
		/// </summary>
		/// <returns></returns>
		private HttpResponseMessage ResponseOK()
		{
			var response = new HttpResponseMessage();
			response.Content = new StringContent("1|OK");
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}


	}
}