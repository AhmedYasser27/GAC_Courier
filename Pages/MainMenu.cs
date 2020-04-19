using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Pages
{
    public static class MainMenu
    {
        public static class WayBill
        {
            public const string PageName = "WayBill";
            public const string RoleName = "WayBill";
            public const string Path = "/WayBill/Index";
            public const string ControllerName = "WayBill";
            public const string ActionName = "Index";
        }
        public static class Delivery
        {
            public const string PageName = "Courier Statement";
            public const string RoleName = "Delivery";
            public const string Path = "/Delivery/Index";
            public const string ControllerName = "Delivery";
            public const string ActionName = "Index";
        }
        public static class Courier
        {
            public const string PageName = "Courier Request";
            public const string RoleName = "Courier";
            public const string Path = "/Courier/Index";
            public const string ControllerName = "Courier";
            public const string ActionName = "Index";
        }

        public static class User
        {
            public const string PageName = "User";
            public const string RoleName = "User";
            public const string Path = "/UserRole/Index";
            public const string ControllerName = "UserRole";
            public const string ActionName = "Index";
        }

        public static class ChangePassword
        {
            public const string PageName = "Change Password";
            public const string RoleName = "Change Password";
            public const string Path = "/UserRole/ChangePassword";
            public const string ControllerName = "UserRole";
            public const string ActionName = "ChangePassword";
        }

        public static class Role
        {
            public const string PageName = "Role";
            public const string RoleName = "Role";
            public const string Path = "/UserRole/Role";
            public const string ControllerName = "UserRole";
            public const string ActionName = "Role";
        }

        public static class ChangeRole
        {
            public const string PageName = "Change Role";
            public const string RoleName = "Change Role";
            public const string Path = "/UserRole/ChangeRole";
            public const string ControllerName = "UserRole";
            public const string ActionName = "ChangeRole";
        }

        public static class Dashboard
        {
            public const string PageName = "Dashboard Main";
            public const string RoleName = "Dashboard Main";
            public const string Path = "/Dashboard/Index";
            public const string ControllerName = "Dashboard";
            public const string ActionName = "Index";
        }

    }
}
