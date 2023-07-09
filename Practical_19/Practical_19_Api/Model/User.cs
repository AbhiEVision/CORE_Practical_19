using System.ComponentModel.DataAnnotations;

namespace Practical_19_Api.Model
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required]
		[StringLength(50)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Password { get; set; }
	}
}