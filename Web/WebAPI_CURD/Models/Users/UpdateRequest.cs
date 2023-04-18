using System.ComponentModel.DataAnnotations;
using WebAPI_CURD.Entities;

namespace WebAPI_CURD.Models.Users
{
    public class UpdateRequest
    {
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EnumDataType(typeof(Role), ErrorMessage = "Role取值为 Admin、Role")]
        public string? Role { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // treat empty string as null for password fields to 
        // make them optional in front end apps
        private string? _password;
        //[MinLength(6)]
        [RegularExpression(RegValidate.PwdReg, ErrorMessage = RegValidate.PwdRegErrMsg)]
        public string? Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        private string? _confirmPassword;
        [Compare("Password")]
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        // helpers

        private string? replaceEmptyWithNull(string? value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
