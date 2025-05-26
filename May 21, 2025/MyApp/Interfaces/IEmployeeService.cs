using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Models;
using MyApp.Models;

namespace MyApp.Interfaces
{
    public interface IEmployeeService
    {
        int AddEmployee(Employee employee);
        List<Employee>? SearchEmployee(SearchModel searchModel);
    }
}