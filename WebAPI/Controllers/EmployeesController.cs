using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeAccess;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (MyEntities entities = new MyEntities())
            {
                return entities.Employee.Include(e => e.Department).ToList();
            }
        }

        public IEnumerable<Employee> GetByDepartmentId(int depId)
        {
            using (MyEntities entities = new MyEntities())
            {
                return entities.Employee.Where(e => e.departmentNo == depId).ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (MyEntities entities = new MyEntities())
            {
                var entity = entities.Employee.Include(e => e.Department).FirstOrDefault(e => e.employeeNo == id);
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

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (MyEntities entities = new MyEntities())
                {
                    employee.employeeNo = entities.Employee.Max(e => e.employeeNo) + 1;
                    entities.Employee.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.employeeNo.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(MyEntities entities = new MyEntities())
                {
                    var entity = entities.Employee.FirstOrDefault(e => e.employeeNo == id);
                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id + " not found to delete!");
                    }
                    else
                    {
                        entities.Employee.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put (int id, [FromBody]Employee employee)
        {
            try
            {
                using (MyEntities entities = new MyEntities())
                {
                    var entity = entities.Employee.FirstOrDefault(e => e.employeeNo == id);
                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id= " + id + " not found!");
                    }
                    else
                    {
                        entity.employeeName = employee.employeeName;
                        entity.salary = employee.salary;
                        entity.departmentNo = employee.departmentNo;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
