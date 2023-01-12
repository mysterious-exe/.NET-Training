using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace recursion
{
    public class Employee
    {
        int empId;
        string name;
        Nullable<int> managerId;
        string managerName = string.Empty;
        int level = 0;

        public static String getName(List<Employee> empList, int? id)
        {
            foreach (Employee emp in empList)
            {
                if (emp.empId == id)
                {
                    return emp.name;
                }
            }
            return "";
        }

        public static int getManager(List<Employee> empList, Employee? e, int level)
        {
            // Console.WriteLine(e.empId + e.name);
            if (e.managerId == null)
            {
                return level;
            }
            else
            {
                foreach (Employee emp in empList)
                {
                    if (emp.empId == e.managerId)
                    {
                        return getManager(empList, emp, level + 1);
                    }
                }
                return level;
            }
        }

        public static void getHierarchy(List<Employee> employeeList)
        {
            foreach (Employee emp in employeeList)
            {
                int level = getManager(employeeList, emp, 0);
                if (emp.managerId != null)
                {
                    emp.managerName = getName(employeeList, emp.managerId);
                }
                emp.level = level;
            }
        }

        public static void Main(string[] args)
        {

            List<Employee> empList = new List<Employee>()
            {
                new Employee { empId = 25, name = "Salman", managerId = null },
                new Employee { empId = 26, name = "Ranbeer", managerId = 25 },
                new Employee { empId = 27, name = "Hrithik", managerId = 25 },
                new Employee { empId = 28, name = "Aamir", managerId = 27 },
                new Employee { empId = 29, name = "Shahid", managerId = 28 },
                new Employee { empId = 30, name = "Sidharth", managerId = null },
                new Employee { empId = 31, name = "Varun", managerId = 30 },
                new Employee { empId = 32, name = "Kabeer", managerId = 30 },
                new Employee { empId = 33, name = "Raj", managerId = 29 },
            };
            getHierarchy(empList);
            foreach (Employee emp in empList)
            {
                Console.WriteLine("{0,-15} {1,-5} {2,-5} {3, -10} {4,-2}", emp.name, emp.empId, emp.managerId, emp.managerName, emp.level);
            }
        }
    }
}