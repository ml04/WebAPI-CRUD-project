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
    /// <summary>
    /// Interaction logic for Add_Edit_Window.xaml
    /// </summary>
    public partial class Add_Edit_Window : Window
    {
        private bool EditState;
        private  EmployeeVM CurrentEmployee;
        private int departmentNo;
        private static APICalls apiCalls = new APICalls();

        public EmployeeVM createdEmployee;
        public EmployeeVM updatedEmployee;

        public Add_Edit_Window(int departmentNo, bool editstate=false, EmployeeVM sendEmployee=null)
        {
            InitializeComponent();
            EditState = editstate;
            CurrentEmployee = sendEmployee;
            this.departmentNo = departmentNo;
            this.DataContext = CurrentEmployee;

            if (!EditState)
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var employee = new EmployeeVM();
            employee.employeeName = txtName.Text;
            employee.salary = double.Parse(txtSalary.Text);
            employee.departmentNo = departmentNo;

            HttpResponseMessage response = apiCalls.Put_PostCall(EditState, CurrentEmployee, employee);

            if (response.IsSuccessStatusCode)
            {
                if (!EditState)
                {
                    MessageBox.Show("Employee added successfully!");
                }

                else
                {
                    MessageBox.Show("Employee edited successfully!");
                }

                this.DialogResult = true;
                this.Close();
            }

            else
            {
                this.DialogResult = false;
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }

            var createdEmployee = response.Content.ReadAsAsync<EmployeeVM>().Result;
            this.createdEmployee = (EmployeeVM)createdEmployee;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = apiCalls.DeleteCall(CurrentEmployee);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Employee with ID: " + CurrentEmployee.employeeNo + " deleted!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
