using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Practical_19.Models;
using System.Net;

namespace Practical_19.Controllers
{
	public class HomeController : Controller
	{

		private readonly HttpClient _httpClient;

		public HomeController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Login()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			var result = await _httpClient.PostAsJsonAsync("https://localhost:7078/api/Access/Login", model);

			var data = await result.Content.ReadAsStringAsync();

			ResponseResult response = JsonConvert.DeserializeObject<ResponseResult>(data);


			if (result.StatusCode == (HttpStatusCode)200 && response.IsSuccess)
			{
				return RedirectToAction("Index");
			}

			if (response.Errors != null)
			{
				foreach (var item in response.Errors)
				{
					ModelState.AddModelError("", item);
				}
			}

			ViewBag.Message = response.Messgae;

			return View(model);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{

			var result = await _httpClient.PostAsJsonAsync("https://localhost:7078/api/Access/SignIn", model);

			var data = await result.Content.ReadAsStringAsync();

			ResponseResult response = JsonConvert.DeserializeObject<ResponseResult>(data);



			if (result.StatusCode == (HttpStatusCode)200 && response.IsSuccess)
			{
				return RedirectToAction("Index");
			}

			if (response.Errors != null)
			{
				foreach (var item in response.Errors)
				{
					ModelState.AddModelError("", item);
				}

			}

			ViewBag.Message = response.Messgae;

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Logout(LogoutModel model)
		{
			var result = await _httpClient.PostAsJsonAsync("https://localhost:7078/api/Access/Logout", model);

			var data = await result.Content.ReadAsStringAsync();

			ResponseResult response = JsonConvert.DeserializeObject<ResponseResult>(data);


			if (result.StatusCode == (HttpStatusCode)200 && response.IsSuccess)
			{
				return RedirectToAction("Index");
			}



			ViewBag.Message = response.Messgae;

			return View(model);
		}
	}
}