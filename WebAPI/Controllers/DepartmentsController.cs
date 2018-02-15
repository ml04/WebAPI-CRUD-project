using EmployeeAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    public class DepartmentsController : ApiController
    {
        public IEnumerable<Department> Get()
        {
            using (MyEntities entities = new MyEntities())
            {
                return entities.Department.Include(e => e.Employee).ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (MyEntities entities = new MyEntities())
            {
                var entity = entities.Department.Include(e => e.Employee).FirstOrDefault(e => e.departmentNo == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID" + id.ToString() + " not found.");
                }
            }
        }
    }
}
