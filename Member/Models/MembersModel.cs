using System.ComponentModel.DataAnnotations;

namespace Member.Models
{
	public class MembersModel
	{
		[Key]
        public int Id { get; set; }

		[Required(ErrorMessage = "帳號為必填")]
		public string Account { get; set; } = string.Empty;

		[Required(ErrorMessage = "密碼為必填")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "信箱為必填")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "電話為必填")]
		public string Phone { get; set; } = string.Empty;

    }
}
