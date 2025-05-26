using MyApp.Models;
using MyApp.Interfaces;
using MyApp.Repositories;
using MyApp.Services;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IRepository<int, Employee> employeeRepository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(employeeRepository);
            ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            manageEmployee.Start();
        }
    }
}