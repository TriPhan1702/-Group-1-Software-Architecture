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

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class AdminController : ApiController
    {
        public ComicNowEntities Context;

        public AdminController()
        {
            Context = new ComicNowEntities();
        }
    }
}
