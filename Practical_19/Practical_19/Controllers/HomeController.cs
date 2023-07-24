using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Practical_19.Models;
using System.Net;
using System.Net.Http.Headers;

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
			HttpClient client = new HttpClient();

			List<RegisterdUser> response = new List<RegisterdUser>();

			if (Request.Cookies["Test"] != null)
			{
				var token = Request.Cookies["Test"].ToString();

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var result = await client.GetAsync("https://localhost:7078/api/Access/Users");

				var data = await result.Content.ReadAsStringAsync();

				response = JsonConvert.DeserializeObject<List<RegisterdUser>>(data);

			}

			if (response == null)
			{
				ViewBag.Messgage = "You are UnAuthorized to see the data";
				response = new List<RegisterdUser>();
			}

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

			//ViewBag.Message = response.Messgae;

			return View(model);
		}

		public IActionResult Register()
		{
			ViewBag.Roles = new List<SelectListItem>() {
				new SelectListItem() {Text ="Admin", Value ="Admin"},
				new SelectListItem() {Text ="User", Value ="User"}
			};

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
			ViewBag.Roles = new List<SelectListItem>() {
				new SelectListItem() {Text ="Admin", Value ="Admin"},
				new SelectListItem() {Text ="User", Value ="User"}
			};
			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			if (Request.Cookies["User"] == null)
			{
				return RedirectToAction("Index");
			}

			Response.Cookies.Delete("User");
			Response.Cookies.Delete("Test");

			return RedirectToAction("Index");
		}


		//public async Task<List<RegisterdUser>> TestEx()
		//{
		//	HttpClient client = new HttpClient();

		//	if (Request.Cookies["Test"] != null)
		//	{
		//		var token = Request.Cookies["Test"].ToString();

		//		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		//		var result = await client.GetAsync("https://localhost:7078/api/Access/Users");

		//		var data = await result.Content.ReadAsStringAsync();

		//		List<RegisterdUser> response = JsonConvert.DeserializeObject<List<RegisterdUser>>(data);

		//		return response;

		//	}

		//	return new List<RegisterdUser>();

		//}

	}
}