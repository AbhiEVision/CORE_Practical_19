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

		public async Task<IActionResult> Index()
		{
			var result = await _httpClient.GetAsync("https://localhost:7078/api/Access/Users");

			var data = await result.Content.ReadAsStringAsync();

			List<RegisterdUser> response = JsonConvert.DeserializeObject<List<RegisterdUser>>(data);


			if (Request.Cookies["User"] != null && response == null)
			{
				ViewBag.Messgage = "You are unauthorize to see the data! ";
				return View(new List<RegisterdUser>());

			}


			if (response == null)
			{
				ViewBag.Messgage = "You are not logged in for See the data";
				return View(new List<RegisterdUser>());
			}

			//List<RegisterdUser> response = JsonConvert.DeserializeObject<List<RegisterdUser>>(data);

			return View(response);
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
				Response.Cookies.Append("Test", response.TokenAsAString);
				Response.Cookies.Append("User", response.UserId);
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
			if (Request.Cookies["User"] == null)
			{
				return RedirectToAction("Index");
			}

			var userId = Request.Cookies["User"].ToString();
			var result = await _httpClient.PostAsJsonAsync("https://localhost:7078/api/Access/Logout", new LogoutModel() { Email = userId });

			var data = await result.Content.ReadAsStringAsync();

			ResponseResult response = JsonConvert.DeserializeObject<ResponseResult>(data);



			if (result.StatusCode == (HttpStatusCode)200 && response.IsSuccess)
			{
				Response.Cookies.Delete("Test");
				Response.Cookies.Delete("User");

			}
			return RedirectToAction("Index");



			//return View();
		}


	}
}