﻿using Microsoft.AspNetCore.Mvc;
using Practical_19_Api.Interfaces.Services;
using Practical_19_Api.Model;
using Practical_19_Api.VIewModel;
using System.Net;

namespace Practical_19_Api.Controllers
{
	[Route("api/{controller}")]
	[ApiController]
	public class AccessController : ControllerBase
	{

		public readonly IUserServices _userServices;

		public AccessController(IUserServices userServices)
		{
			_userServices = userServices;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApiResponseObject test = await _userServices.LoginUserAsync(model);

				if (test == null)
				{
					return StatusCode((int)HttpStatusCode.InternalServerError, "No Response");
				}

				if (test.IsSuccess)
				{
					return StatusCode((int)HttpStatusCode.Accepted, test);
				}
				else
				{
					return StatusCode((int)HttpStatusCode.Unauthorized, test);
				}

			}

			var obj = new ApiResponseObject()
			{
				Messgae = "Model Validation Error",
				IsSuccess = false,
			};

			return StatusCode((int)HttpStatusCode.BadRequest, obj);

		}

		[HttpPost("SignIn")]
		public async Task<IActionResult> SignIn(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApiResponseObject test = await _userServices.RegisterUserAsync(model);

				if (test == null)
				{
					return StatusCode((int)HttpStatusCode.InternalServerError, "No Response");
				}

				if (test.IsSuccess)
				{
					return StatusCode((int)HttpStatusCode.Accepted, test);
				}
				else
				{
					return StatusCode((int)HttpStatusCode.BadRequest, test);
				}

			}

			var obj = new ApiResponseObject()
			{
				Messgae = "Model Validation Error",
				IsSuccess = false,
			};

			return StatusCode((int)HttpStatusCode.BadRequest, obj);
		}

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout(LogoutModel model)
		{
			if (ModelState.IsValid)
			{
				var test = await _userServices.LogoutUserAsync(model);

				return StatusCode((int)HttpStatusCode.Accepted, test);
			}

			return StatusCode((int)HttpStatusCode.BadGateway, "Model Invalid!");
		}

	}
}
