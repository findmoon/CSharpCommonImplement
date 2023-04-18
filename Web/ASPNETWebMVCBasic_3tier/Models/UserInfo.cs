using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPNETWebMVCBasic.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        [Display(Name = "用户名")]
        public string Name { get; set;}
        [Display(Name ="密码")]
        [Required]
        public string Password { get; set;}

        [Display(Name = "邮箱")]
        public string Email { get; set;}

        [Display(Name = "手机号")]
        public string PhoneNum { get; set; }
    }
}