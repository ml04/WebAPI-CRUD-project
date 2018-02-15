using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    class APICalls
    {
        public HttpClient ClientConnect()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:27709/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public HttpResponseMessage Put_PostCall(bool EditState, EmployeeVM CurrentEmployee, EmployeeVM employee)
        {
            HttpClient client = ClientConnect();

            HttpResponseMessage response;

            response = (EditState) ? client.PutAsJsonAsync("api/Employees/" + CurrentEmployee.employeeNo, employee).Result : client.PostAsJsonAsync("api/Employees", employee).Result;

            return response;
        }

        public HttpResponseMessage DeleteCall(EmployeeVM CurrentEmployee)
        {
            HttpClient client = ClientConnect();
            HttpResponseMessage response = client.DeleteAsync("api/Employees/" + CurrentEmployee.employeeNo).Result;
            return response;
        }

        public HttpResponseMessage GetDepartmentCall()
        {
            HttpClient client = ClientConnect();
            HttpResponseMessage response = client.GetAsync("api/departments").Result;
            return response;
        }

        public HttpResponseMessage GetEmployee(int departmentNo)
        {
            HttpClient client = ClientConnect();
            HttpResponseMessage response = client.GetAsync("api/Employees?depId=" + departmentNo).Result;
            return response;
        }

    }
}
