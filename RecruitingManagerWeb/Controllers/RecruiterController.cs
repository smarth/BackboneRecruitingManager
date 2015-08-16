using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HR.DataProvider;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using RecruitingManagerWeb.Models;

namespace RecruitingManagerWeb.Controllers
{
    public class RecruiterController : ApiController
    {
       static DataProvider dp;
        static RecruiterController()
        {
            string sqlConnString = ConfigurationManager.ConnectionStrings["HRDB"].ConnectionString;
            dp = new DataProvider(sqlConnString);
            
        }
        // GET api/<controller>
        public IEnumerable<Recruiter> Get()
        {
            try{
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                DataTable dt = dp.ExecuteReader("dbo.usp_ReadRecruiters", sqlParameters.ToArray());
                List<Recruiter> recruiters = new List<Recruiter>();
                foreach (DataRow dr in dt.Rows)
                {
                    Recruiter rt = new Recruiter();
                    rt.Id = (int)dr["Id"];
                    rt.Name = (string)dr["Name"];
                    recruiters.Add(rt);
                }
                return recruiters;
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return null;
        }

        // POST api/<controller>
        public Recruiter Post(Recruiter recruiter)
        {
            String name = recruiter.Name;
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Empty Name");
            }
            try
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("name", name));
                DataTable dt = dp.ExecuteReader("dbo.usp_CreateRecruiter", sqlParams.ToArray());
                int Id = Int32.Parse(dt.Rows[0]["ID"].ToString());

                recruiter.Id = Id;
                return recruiter;
      
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id,Recruiter recruiter)
        {

            string name = recruiter.Name;
            if (String.IsNullOrEmpty(name) || id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("id", id));
                sqlParams.Add(new SqlParameter("name", name));
               
                dp.ExecuteCrud("dbo.usp_UpdateRecruiter", sqlParams.ToArray());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("id", id));
                dp.ExecuteCrud("dbo.usp_DeleteRecruiter", sqlParams.ToArray());
                return Request.CreateResponse(HttpStatusCode.OK, String.Format("{{\"Id\":{0}}}", id));
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
        } 
        
    }
}