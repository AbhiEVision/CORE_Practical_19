using Practical_19_Api.Data;
using Practical_19_Api.Interfaces.Repository;
using Practical_19_Api.Model;

namespace Practical_19_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _db;

		public UserRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task AddUser(User user)
		{
			_db.Users.Add(user);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteUser(User user)
		{
			_db.Users.Remove(user);
			await _db.SaveChangesAsync();

		}
	}
}
