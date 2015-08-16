using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingManagerWeb.Models
{
    public class Candidate
    {
        public int Id {get; set;}
        public int? RecruiterId { get; set; }
        public String Name { get; set; }
        public String RecruiterName { get; set; }

    }
}