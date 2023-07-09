using Practical_19_Api.Model;
using Practical_19_Api.VIewModel;

namespace Practical_19_Api.Interfaces.Services
{
	public interface IUserServices
	{
		Task<ApiResponseObject> RegisterUserAsync(RegisterViewModel model);

		Task<ApiResponseObject> LoginUserAsync(LoginViewModel model);

		Task<ApiResponseObject> LogoutUserAsync(LogoutModel model);

		Task<bool> IsUserLoggedIn(LoginViewModel model);

		IEnumerable<RegistredUser> GetUsers();
	}
}
