using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class RolesController : ApiController
    {
        public RoleServices RoleServices;

        public RolesController()
        {
            RoleServices = new RoleServices();
        }

        //GET api/roles
        //Get a list of all the roles
        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            var roles = RoleServices.GetAllRoles();

            if (!roles.Any())
            {
                return NotFound();
            }

            return Ok(roles.ToList().Select(Mapper.Map<Role, RoleDto>));
        }
        

    }
}
