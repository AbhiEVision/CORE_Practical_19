using Practical_19_Api.VIewModel;

namespace Practical_19_Api.Interfaces.Repository
{
	public interface IAccessRepository
	{
		Task AddAccessToUser(LoginViewModel model);

		Task AddAccessToAdmin(LoginViewModel model);

		Task RemoveAccess(LogoutModel model);

		Task<bool> IsUserLoggedIn(LogoutModel model);
	}
}
