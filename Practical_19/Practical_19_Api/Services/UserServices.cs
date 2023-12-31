﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Practical_19_Api.Interfaces.Repository;
using Practical_19_Api.Interfaces.Services;
using Practical_19_Api.Model;
using Practical_19_Api.VIewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practical_19_Api.Services
{
	public class UserServices : IUserServices
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IAccessRepository _accessRepository;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;

		public UserServices(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IAccessRepository access,
			SignInManager<IdentityUser> signInManager,
			IUserRepository userRepository,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_accessRepository = access;
			this._signInManager = signInManager;
			_userRepository = userRepository;
			_configuration = configuration;
		}


		public async Task<ApiResponseObject> LoginUserAsync(LoginViewModel model)
		{
			//var isAlreadyLoggedIn = await IsUserLoggedIn(model);

			//if (isAlreadyLoggedIn)
			//{
			//	await LogoutUserAsync(new LogoutModel() { Email = model.Email });
			//}

			//var user = await _userManager.FindByEmailAsync(model.Email);

			//if (user == null)
			//{
			//	return new ApiResponseObject()
			//	{
			//		IsSuccess = false,
			//		Messgae = "User not found!",
			//	};
			//}

			//var result = await _userManager.CheckPasswordAsync(user, model.Password);

			//if (result == false)
			//{
			//	return new ApiResponseObject()
			//	{
			//		IsSuccess = false,
			//		Messgae = "Invalid Password!",
			//	};
			//}

			//var ListOfClaims = new List<Claim>()
			//{
			//	new Claim("Email",model.Email),
			//};

			//var listOfRole = await _userManager.GetRolesAsync(user);

			//foreach (var role in listOfRole)
			//{
			//	if (role == "Admin")
			//	{
			//		await _accessRepository.AddAccessToAdmin(model);
			//	}

			//	if (role == "User")
			//	{
			//		await _accessRepository.AddAccessToUser(model);
			//	}
			//}

			//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthKey"]));

			////await _signInManager.SignInAsync(user, true);

			//var token = new JwtSecurityToken(
			//		claims: ListOfClaims,
			//		expires: DateTime.Now.AddHours(1),
			//	signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			//	) ;

			//string tokenAsAString = new JwtSecurityTokenHandler().WriteToken(token);

			//return new ApiResponseObject()
			//{
			//	IsSuccess = true,
			//	Messgae = "You are successfully logged in!",
			//	//TokenAsAString = tokenAsAString
			//};

			return await TestLogin(model);

		}

		public async Task<ApiResponseObject> RegisterUserAsync(RegisterViewModel model)
		{
			if (model == null)
			{
				return new ApiResponseObject()
				{
					Messgae = "Model is null",
					IsSuccess = false,
				};
			}

			if (model.Password != model.ConfirmPassword)
			{
				return new ApiResponseObject()
				{
					Messgae = "Password and confirm password is not matched!",
					IsSuccess = false,
				};
			}

			var identityUser = new IdentityUser()
			{
				Email = model.Email,
				UserName = model.Email,
			};

			var result = await _userManager.CreateAsync(identityUser, model.Password);

			if (result.Succeeded)
			{
				if (!await _roleManager.RoleExistsAsync("Admin"))
				{
					await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
				}

				if (!await _roleManager.RoleExistsAsync("User"))
				{
					await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
				}

				var test = await _userManager.AddToRoleAsync(identityUser, model.Roles.ToString());

				User newUser = new User()
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Password = model.Password,
					Roles = model.Roles,
				};

				await _userRepository.AddUser(newUser);


				if (test.Succeeded)
				{
					return new ApiResponseObject()
					{
						Messgae = "User created Successfully and added to user role",
						IsSuccess = true,
					};
				}

				return new ApiResponseObject()
				{
					Messgae = "User created Successfully but it not added in role",
					IsSuccess = true,
				};

			}

			return new ApiResponseObject()
			{
				Messgae = "User is not created!",
				IsSuccess = false,
				Errors = result.Errors.Select(x => x.Description),
			};
		}

		public async Task<ApiResponseObject> LogoutUserAsync(LogoutModel model)
		{


			bool userLoggedIn = await _accessRepository.IsUserLoggedIn(model);

			//await _signInManager.SignOutAsync();

			if (userLoggedIn)
			{
				await _accessRepository.RemoveAccess(model);

				await _signInManager.SignOutAsync();
				return new ApiResponseObject()
				{
					Messgae = "User successfully Logged Out!",
					IsSuccess = true,
				};

			}


			return new ApiResponseObject()
			{
				Messgae = "User is alreay logged out!!",
				IsSuccess = false,
				Errors = new List<string>() { "requested user is not found!" },
			};
		}

		public async Task<bool> IsUserLoggedIn(LoginViewModel model)
		{
			return await _accessRepository.IsUserLoggedIn(new LogoutModel() { Email = model.Email });
		}

		public async Task<List<RegistredUser>> GetUsers()
		{
			List<RegistredUser> list = new List<RegistredUser>();

			foreach (var item in await _userManager.GetUsersInRoleAsync("Admin"))
			{
				list.Add(new RegistredUser()
				{
					Email = item.Email,
					Role = "Admin",
				});
			}

			foreach (var item in await _userManager.GetUsersInRoleAsync("User"))
			{
				list.Add(new RegistredUser()
				{
					Email = item.Email,
					Role = "User",
				});
			}


			return list;
		}

		private string GenerateJWT(LoginViewModel user, string roles)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["AuthKey"]);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new List<Claim>() {
					//new Claim("Id", user.Id),
					new Claim(JwtRegisteredClaimNames.Email, user.Email!),
					//new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT Token Id
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()), //time at which JWT issued
                    new Claim(ClaimTypes.Role , roles)
				}),
				Expires = DateTime.Now.AddMinutes(5),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};
			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwt = jwtTokenHandler.WriteToken(token);
			return jwt;
		}


		public async Task<ApiResponseObject> TestLogin(LoginViewModel model)
		{
			if (model == null)
			{
				return new ApiResponseObject()
				{
					Messgae = "Model is null",
					IsSuccess = false
				};
			}

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				return new ApiResponseObject()
				{
					Messgae = "Requested User is not found!",
					IsSuccess = false
				};
			}

			var isVerified = await _userManager.CheckPasswordAsync(user, model.Password);

			if (!isVerified)
			{
				return new ApiResponseObject()
				{
					Messgae = "Login Credentials is null!",
					IsSuccess = false,
					Errors = new List<string>() { "User password is invalid" }
				};
			}

			var rolesOfUser = await _userManager.GetRolesAsync(user);

			var combinedRoles = string.Join(",", rolesOfUser);

			var token = GenerateJWT(model, combinedRoles);

			return new ApiResponseObject()
			{
				Messgae = "User successfully logged in!",
				IsSuccess = true,
				TokenAsAString = token,
				UserId = model.Email,
			};

		}

	}
}
