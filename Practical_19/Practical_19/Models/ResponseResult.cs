namespace Practical_19.Models
{
	public class ResponseResult
	{
		public string Messgae { get; set; }

		public bool IsSuccess { get; set; }

		public IEnumerable<string> Errors { get; set; }

		public string TokenAsAString { get; set; }

		public string UserId { get; set; }

	}
}
