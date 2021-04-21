using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;

namespace Lab1CSharp.Repo.Interfaces
{
    interface IEmployeeRepo: IRepository<long, Employee>
    {
        Employee GetEmployeeByUser(string user);
        string GetPasswordByUser(string user);
        bool UsernameExists(string user);
        IList<Employee> FilterByName(String name);
        IList<Employee> FilterByPosition(String position);
        IList<Employee> FilterByOffice(long office);
    }
}