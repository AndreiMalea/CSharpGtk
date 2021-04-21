using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;

namespace Lab1CSharp.Repo.Interfaces
{
    interface IOfficeRepo: IRepository<long, Office>
    {
        IList<Office> FilterByLocation(String location);
    }
}