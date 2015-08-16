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
    
    public class CandidateController : ApiController
    {
        static DataProvider dp;
        static CandidateController()
        {
            string sqlConnString = ConfigurationManager.ConnectionStrings["HRDB"].ConnectionString;
            dp = new DataProvider(sqlConnString);
            
        }
        // GET api/values
        public IEnumerable<Candidate> Get()
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                DataTable dt = dp.ExecuteReader("dbo.usp_ReadCandidates", sqlParameters.ToArray());
                List<Candidate> candidates = new List<Candidate>();
                foreach (DataRow dr in dt.Rows)
                {
                    Candidate cd = new Candidate();
                    cd.Id = (int)dr["CandidateId"];
                    cd.Name = (string)dr["CandidateName"];
                    if (dr["RecruiterId"] != DBNull.Value)
                    {
                        cd.RecruiterId = (int?)dr["RecruiterId"];
                        cd.RecruiterName = (string)dr["RecruiterName"];
                    }
                    candidates.Add(cd);
                }
                return candidates;
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }

        }

        // GET api/values/5
        public Candidate Get(int id)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("candidate_id", id));
                DataTable dt = dp.ExecuteReader("dbo.usp_ReadCandidates", sqlParameters.ToArray());

                if(dt.Rows.Count==1){
                    DataRow dr = dt.Rows[0];
                    Candidate cd = new Candidate();
                    cd.Id = (int)dr["CandidateId"];
                    cd.Name = (string)dr["CandidateName"];
                    if (dr["RecruiterId"] != DBNull.Value)
                    {
                        cd.RecruiterId = (int?)dr["RecruiterId"];
                        cd.RecruiterName = (string)dr["RecruiterName"];
                    }
                    return cd;
                }
                else{
                    return null;
                }
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
        }

        public Candidate Post(Candidate cd)
        {
            String name = cd.Name;
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Empty Name");
            }
            try
            {
                int? recruiterId = null;
                if(cd.RecruiterId !=null)
                   recruiterId = (int)cd.RecruiterId;
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("name",name));
                if (recruiterId > 0)
                {
                    sqlParams.Add(new SqlParameter("recruiter_id", recruiterId));
                }
                DataTable dt = dp.ExecuteReader("dbo.usp_CreateCandidate", sqlParams.ToArray());
                int Id = Int32.Parse(dt.Rows[0]["ID"].ToString());


                cd.Id = Id;
                return cd;

            }
            
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }
            
        }

        public HttpResponseMessage Put(int id,Candidate cd)
        {

            string name = cd.Name;
            if (String.IsNullOrEmpty(name) || id<=0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                int? recruiterId = null;
                if (cd.RecruiterId != null)
                    recruiterId = (int)cd.RecruiterId;
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("id", id));
                sqlParams.Add(new SqlParameter("name", name));
                if (recruiterId > 0)
                {
                    sqlParams.Add(new SqlParameter("recruiter_id", recruiterId));
                }

                dp.ExecuteCrud("dbo.usp_UpdateCandidate", sqlParams.ToArray());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //Log somewhere this exception
                throw new Exception("Server Error");
            }

        }

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
                dp.ExecuteCrud("dbo.usp_DeleteCandidate", sqlParams.ToArray());
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
