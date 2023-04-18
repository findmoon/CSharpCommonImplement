using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPNETWebMVCBasic.ViewModels
{
    /// <summary>
    /// 注册
    /// </summary>
    public class RegisterViewModel
    {

        [Display(Name = "用户名")]
        public string Name { get; set; }

        [Display(Name = "密码")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "密码确认")]
        [Required]
        public string ConfirmPassword { get; set; }


        [Display(Name = "邮箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "手机号")]
        //[StringLength(maximumLength:18,MinimumLength =11,ErrorMessage ="手机号")]
        public string PhoneNum { get; set; }
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
    }
}