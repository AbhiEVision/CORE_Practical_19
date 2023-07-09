using System.ComponentModel.DataAnnotations;

namespace Practical_19_Api.Model
{
	public class AccessTable
	{
		public int Id { get; set; }

		[Required]
		public string Token { get; set; }

		public bool ReadPermission { get; set; } = false;

		public bool CreatePermission { get; set; } = false;

		public bool UpdatePermission { get; set; } = false;

		public bool DeletePermission { get; set; } = false;
	}
}