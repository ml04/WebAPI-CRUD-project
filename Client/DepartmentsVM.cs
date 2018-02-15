using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class DepartmentsVM
    {
        public int departmentNo { get; set; }
        public string departmentName { get; set; }
        public string departmentLocation { get; set; }
        public string departmentDesc { get { return departmentName + ' ' + departmentLocation; } }
    }
}
