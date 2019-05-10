using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManager.BackEnd.Models
{
    public class ServiceDto
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public byte[] image { get; set; }
        public string FullName { get; set; }
        public string phoneNumber { get; set; }
    }
}