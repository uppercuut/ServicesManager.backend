using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManager.BackEnd.Models.Members
{
    public class AddMemberDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MemberTypeAlias { get; set; }
        
    }
}