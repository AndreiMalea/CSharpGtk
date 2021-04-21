using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;

namespace Lab1CSharp.Repo.Interfaces
{
    interface ITransactionRepo : IRepository<int, Transaction>
    {
        
    }
}