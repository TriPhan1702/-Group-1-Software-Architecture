using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ModelView
{
    public class AccountViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public static readonly int UserRoleId = 1;
        public static readonly int AdminRoleId = 2;
    }
}