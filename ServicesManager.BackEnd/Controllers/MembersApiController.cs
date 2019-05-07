using ServicesManager.BackEnd.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace ServicesManager.BackEnd.Controllers
{
    public class MembersApiController : UmbracoApiController
    {
        private IMemberService _memberService;
        public MembersApiController()
        {
            _memberService = Services.MemberService;
        }

        [HttpPost]
        public IHttpActionResult Add(AddMemberDto addMemberDto)
        {
            var Member = _memberService.GetByEmail(addMemberDto.Email);
            if (Member != null)
            {
                return Ok(new { error_code = 1, desc =" ("+ addMemberDto.Email + ") " +"this email is already in use" });
            }

            var newMember = _memberService.CreateMember(addMemberDto.Email, addMemberDto.Email, addMemberDto.FullName, addMemberDto.MemberTypeAlias);
            newMember.SetValue("fullName", addMemberDto.FullName);
            newMember.SetValue("phoneNumber", addMemberDto.PhoneNumber);
            newMember.SetValue("address", addMemberDto.Address);
            newMember.SetValue("sex", addMemberDto.Sex);
            _memberService.Save(newMember);
            _memberService.SavePassword(newMember, addMemberDto.Password);
            return Ok(new {error_code= 0, desc = addMemberDto});
        }

        [HttpPost]
        public IHttpActionResult Authenticate(UserauthenticationDto userauthenticationDto)
        {
          var isValidUser=  Membership.ValidateUser(userauthenticationDto.Username, userauthenticationDto.Password);
            if (!isValidUser)
            {
                return Ok(new { error_code = 2, desc = "the information you entered is not correct" });
            }
           var currentMember = _memberService.GetByUsername(userauthenticationDto.Username);
            var memberDto = new AddMemberDto()
            {
                 FullName = currentMember.GetValue<string>("fullName"),
                 Address = currentMember.GetValue<string>("address"),
                 Email = currentMember.Email,
                 MemberTypeAlias = currentMember.ContentTypeAlias,
                 PhoneNumber = currentMember.GetValue<string>("phoneNumber"),
                 Sex = currentMember.GetValue<string>("sex"),
                  Id = currentMember.Id.ToString()
            };
            return Ok(new { error_code = 0, desc = memberDto });
        }

        [HttpPost]
        public IHttpActionResult UpdateEmail(AddMemberDto addMemberDto)
        {
            if (addMemberDto.Id == null)
            {
                return Ok(new { error_code = 3, desc = "not authorized" });
            }
            var Member =  _memberService.GetById(int.Parse(addMemberDto.Id));
            Member.Username = addMemberDto.Email;
            Member.Email = addMemberDto.Email;
            _memberService.Save(Member);
            return Ok(new { error_code = 0, desc = addMemberDto });
        }


        [HttpPost]
        public IHttpActionResult UpdatePassword(AddMemberDto addMemberDto)
        {
            if (addMemberDto.Id == null)
            {
                return Ok(new { error_code = 3, desc = "not authorized" });
            }
            var Member = _memberService.GetById(int.Parse(addMemberDto.Id)); 
            _memberService.SavePassword(Member, addMemberDto.Password);
            return Ok(new { error_code = 0, desc = addMemberDto });
        }
    }
}
