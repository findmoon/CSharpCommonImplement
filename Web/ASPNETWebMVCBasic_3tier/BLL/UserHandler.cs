﻿using ASPNETWebInstall.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebInstall.BLL
{
    public class UserHandler
    {
        public bool IsValidUser(LoginViewModel u)
        {
            if (u.Name.ToLower() == "admin" && u.Password == "abc123")
            {
                return true;
            }
            return false;
        }
    }
}