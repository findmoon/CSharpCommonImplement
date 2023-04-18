using ASPNETWebMVCBasic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ASPNETWebMVCBasic.ViewModels
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginViewModel: UserInfo
    {
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
    }
}