using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class RoleServices
    {
        public ComicNowEntities Context;

        public RoleServices()
        {
            Context = new ComicNowEntities();
        }

        public List<Role> GetAllRoles()
        {
            return Context.Roles.ToList();
        }
    }
}