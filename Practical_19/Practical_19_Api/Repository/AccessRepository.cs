using Microsoft.EntityFrameworkCore;
using Practical_19_Api.Data;
using Practical_19_Api.Interfaces.Repository;
using Practical_19_Api.Model;
using Practical_19_Api.VIewModel;

namespace Practical_19_Api.Repository
{
	public class AccessRepository : IAccessRepository
	{
		private readonly AppDbContext _db;

		public AccessRepository(AppDbContext db)
		{
			_db = db;
		}
		public async Task AddAccessToAdmin(LoginViewModel model)
		{
			if (_db.Access.Any(x => x.Token == model.Email))
			{
				_db.Access.Remove(_db.Access.First(x => x.Token == model.Email));
				await _db.SaveChangesAsync();
			}


			var access = new AccessTable()
			{
				Token = model.Email,
				CreatePermission = true,
				DeletePermission = true,
				ReadPermission = true,
				UpdatePermission = true,
			};
			_db.Access.Add(access);
			await _db.SaveChangesAsync();
		}

		public async Task AddAccessToUser(LoginViewModel model)
		{

			if (_db.Access.Any(x => x.Token == model.Email))
			{
				return;
			}

			var access = new AccessTable()
			{
				Token = model.Email,
				CreatePermission = false,
				DeletePermission = false,
				ReadPermission = true,
				UpdatePermission = false,
			};
			_db.Access.Add(access);
			await _db.SaveChangesAsync();
		}

		public async Task RemoveAccess(LogoutModel model)
		{
			var access = _db.Access.FirstOrDefault(x => x.Token == model.Email);

			if (access != null)
			{
				_db.Access.Remove(access);
			}

			await _db.SaveChangesAsync();
		}

		public async Task<bool> IsUserLoggedIn(LogoutModel model)
		{
			var user = await _db.Access.FirstOrDefaultAsync(x => x.Token == model.Email);

			if (user != null)
			{
				return true;
			}

			return false;
		}
	}
}
