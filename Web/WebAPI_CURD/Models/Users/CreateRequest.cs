using System.ComponentModel.DataAnnotations;
using WebAPI_CURD.Entities;

namespace WebAPI_CURD.Models.Users
{
    /// <summary>
    /// 创建的请求
    /// </summary>
    public class CreateRequest
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EnumDataType(typeof(Role),ErrorMessage = "Role取值为 Admin、Role")]
        public string? Role { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        //[MinLength(6)]
        [RegularExpression(RegValidate.PwdReg, ErrorMessage = RegValidate.PwdRegErrMsg)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
