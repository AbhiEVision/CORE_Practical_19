using Practical_19_Api.Model;

namespace Practical_19_Api.Interfaces.Repository
{
	public interface IUserRepository
	{
		Task AddUser(User user);

		Task DeleteUser(User user);
	}
}
