using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace ServicesManager.BackEnd.Controllers
{
    public class KhalidApiController : UmbracoApiController
    {
        public IHttpActionResult AddMember()
        {
          var service =    Services.MemberService;

            service.CreateMember(,)


            return Ok();
        }


    }
}