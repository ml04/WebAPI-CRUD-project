using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DepartmentsVM department = new DepartmentsVM();
        private EmployeeVM employee = new EmployeeVM();
        private List<EmployeeVM> employees = new List<EmployeeVM>();
        private APICalls apiCalls = new APICalls();
    
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindDepartmentsList();
            if (employees.Count > 0)
            {
                MyGrid.SelectedItem = MyGrid.Items[0];
            }
            MyGrid.Focus();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BindDepartmentsList()
        {
            HttpResponseMessage response = apiCalls.GetDepartmentCall();

            if (response.IsSuccessStatusCode)
            {
                var departments = response.Content.ReadAsAsync<IEnumerable<DepartmentsVM>>().Result;
                cbox.ItemsSource = departments;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }


        private void cbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            department = (DepartmentsVM)cbox.SelectedItem;
            HttpResponseMessage response = apiCalls.GetEmployee(((DepartmentsVM)cbox.SelectedItem).departmentNo);
            if (response.IsSuccessStatusCode)
            {
                employees = response.Content.ReadAsAsync<IEnumerable<EmployeeVM>>().Result.ToList();
                btn_Edit.IsEnabled = (employees.Count > 0) ? true : false;
                MyGrid.ItemsSource = employees;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add_Edit_Window view = new Add_Edit_Window(department.departmentNo);
            view.ShowDialog();
            if (view.DialogResult.Value)
            {
                employees.Add(view.createdEmployee);
                MyGrid.Items.Refresh();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void MyGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (employees.Count > 0 && MyGrid.SelectedItem != null)
            {
                Edit();
            }
            else
            {
                MessageBox.Show("Please, select Employee");
            }
        }

        private void Edit()
        {
            employee = (EmployeeVM)MyGrid.SelectedItem;
            Add_Edit_Window view = new Add_Edit_Window(department.departmentNo, true, employee);
            view.ShowDialog();
            if (view.DialogResult.Value)
            {
                if (view.createdEmployee != null)
                {
                    employee.employeeName = view.createdEmployee.employeeName;
                    employee.salary = view.createdEmployee.salary;
                }
                else
                {
                    employees.Remove(employee);
                }
                MyGrid.Items.Refresh();
            }
        }
    }
    
}
